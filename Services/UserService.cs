using Microsoft.EntityFrameworkCore;
using MLMApp.Data;
using MLMApp.Models;

namespace MLMApp.Services
{
    /// <summary>
    /// Service for user-related operations
    /// </summary>
    public interface IUserService
    {
        Task<string> GenerateUserIdAsync();
        Task<bool> ValidateSponsorIdAsync(string? sponsorId);
        Task<User?> RegisterUserAsync(RegisterViewModel model);
        Task<User?> AuthenticateUserAsync(string email, string password);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUserIdAsync(string userId);
        Task<List<User>> GetDirectReferralsAsync(string userId);
        Task<int> GetTotalTeamMembersAsync(string userId, int maxLevels = 3);
        Task<decimal> CalculateTotalIncomeAsync(string userId);
        Task<List<GenerationLevelViewModel>> GetGenerationLevelsAsync(string userId, int maxLevels = 3);
        Task<TreeNodeViewModel> GetGenerationTreeAsync(string userId, int maxLevels = 3);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserStatusAsync(int userId, bool isActive);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Generate unique User ID (e.g., REG1001)
        /// </summary>
        public async Task<string> GenerateUserIdAsync()
        {
            var lastUser = await _context.Users
                .OrderByDescending(u => u.Id)
                .FirstOrDefaultAsync();

            int nextNumber = 1001;
            if (lastUser != null && lastUser.UserId.StartsWith("REG"))
            {
                var numberPart = lastUser.UserId.Substring(3);
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"REG{nextNumber}";
        }

        /// <summary>
        /// Validate if Sponsor ID exists
        /// </summary>
        public async Task<bool> ValidateSponsorIdAsync(string? sponsorId)
        {
            if (string.IsNullOrWhiteSpace(sponsorId))
                return true; // Sponsor ID is optional

            return await _context.Users
                .AnyAsync(u => u.UserId == sponsorId && u.IsActive);
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public async Task<User?> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                // Check if email already exists
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    return null;
                }

                // Check if mobile number already exists
                if (await _context.Users.AnyAsync(u => u.MobileNumber == model.MobileNumber))
                {
                    return null;
                }

                // Validate sponsor ID if provided
                if (!string.IsNullOrWhiteSpace(model.SponsorId))
                {
                    if (!await ValidateSponsorIdAsync(model.SponsorId))
                    {
                        return null;
                    }
                }

                // Generate User ID
                var userId = await GenerateUserIdAsync();

                // Hash password (simple hash for demo - use proper hashing in production)
                var passwordHash = HashPassword(model.Password);

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    UserId = userId,
                    Password = passwordHash,
                    SponsorId = string.IsNullOrWhiteSpace(model.SponsorId) ? null : model.SponsorId,
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return null;
            }
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Sponsor)
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

                if (user == null)
                    return null;

                // Verify password (simple hash for demo - use proper hashing in production)
                if (VerifyPassword(password, user.Password))
                {
                    return user;
                }

                return null;
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Database connection error during authentication");
                throw; // Re-throw to be handled by controller
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication");
                throw; // Re-throw to be handled by controller
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Sponsor)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Get user by User ID
        /// </summary>
        public async Task<User?> GetUserByUserIdAsync(string userId)
        {
            return await _context.Users
                .Include(u => u.Sponsor)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// Get direct referrals for a user
        /// </summary>
        public async Task<List<User>> GetDirectReferralsAsync(string userId)
        {
            return await _context.Users
                .Where(u => u.SponsorId == userId && u.IsActive)
                .OrderBy(u => u.RegistrationDate)
                .ToListAsync();
        }

        /// <summary>
        /// Get total team members up to specified levels
        /// </summary>
        public async Task<int> GetTotalTeamMembersAsync(string userId, int maxLevels = 3)
        {
            var allMembers = new HashSet<string>();
            var queue = new Queue<(string id, int level)>();
            queue.Enqueue((userId, 0));

            while (queue.Count > 0)
            {
                var (currentId, level) = queue.Dequeue();

                if (level >= maxLevels)
                    continue;

                var referrals = await GetDirectReferralsAsync(currentId);
                foreach (var referral in referrals)
                {
                    if (allMembers.Add(referral.UserId))
                    {
                        queue.Enqueue((referral.UserId, level + 1));
                    }
                }
            }

            return allMembers.Count;
        }

        /// <summary>
        /// Calculate total income based on generation levels
        /// Level 1: ₹100 per direct referral
        /// Level 2: ₹50 per member
        /// Level 3: ₹25 per member
        /// </summary>
        public async Task<decimal> CalculateTotalIncomeAsync(string userId)
        {
            decimal totalIncome = 0;

            // Level 1: Direct referrals - ₹100 each
            var level1Referrals = await GetDirectReferralsAsync(userId);
            totalIncome += level1Referrals.Count * 100;

            // Level 2: ₹50 per member
            foreach (var level1User in level1Referrals)
            {
                var level2Referrals = await GetDirectReferralsAsync(level1User.UserId);
                totalIncome += level2Referrals.Count * 50;
            }

            // Level 3: ₹25 per member
            foreach (var level1User in level1Referrals)
            {
                var level2Referrals = await GetDirectReferralsAsync(level1User.UserId);
                foreach (var level2User in level2Referrals)
                {
                    var level3Referrals = await GetDirectReferralsAsync(level2User.UserId);
                    totalIncome += level3Referrals.Count * 25;
                }
            }

            return totalIncome;
        }

        /// <summary>
        /// Get generation levels with details
        /// </summary>
        public async Task<List<GenerationLevelViewModel>> GetGenerationLevelsAsync(string userId, int maxLevels = 3)
        {
            var levels = new Dictionary<int, GenerationLevelViewModel>();
            decimal[] incomePerMember = { 100, 50, 25 };

            var queue = new Queue<(string id, int level)>();
            queue.Enqueue((userId, 0));

            while (queue.Count > 0)
            {
                var (currentId, level) = queue.Dequeue();

                if (level >= maxLevels)
                    continue;

                var referrals = await GetDirectReferralsAsync(currentId);
                
                if (referrals.Count > 0)
                {
                    int levelNumber = level + 1;
                    
                    if (!levels.ContainsKey(levelNumber))
                    {
                        levels[levelNumber] = new GenerationLevelViewModel
                        {
                            Level = levelNumber,
                            MemberCount = 0,
                            IncomePerMember = incomePerMember[level],
                            TotalIncome = 0,
                            Members = new List<User>()
                        };
                    }

                    levels[levelNumber].MemberCount += referrals.Count;
                    levels[levelNumber].TotalIncome += referrals.Count * incomePerMember[level];
                    levels[levelNumber].Members.AddRange(referrals);

                    foreach (var referral in referrals)
                    {
                        queue.Enqueue((referral.UserId, level + 1));
                    }
                }
            }

            return levels.Values.OrderBy(l => l.Level).ToList();
        }

        /// <summary>
        /// Get generation tree structure
        /// </summary>
        public async Task<TreeNodeViewModel> GetGenerationTreeAsync(string userId, int maxLevels = 3)
        {
            var user = await GetUserByUserIdAsync(userId);
            if (user == null)
                return new TreeNodeViewModel();

            var treeNode = new TreeNodeViewModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive
            };

            await BuildTreeRecursiveAsync(treeNode, 0, maxLevels);
            return treeNode;
        }

        private async Task BuildTreeRecursiveAsync(TreeNodeViewModel node, int currentLevel, int maxLevels)
        {
            if (currentLevel >= maxLevels)
                return;

            var referrals = await GetDirectReferralsAsync(node.UserId);
            foreach (var referral in referrals)
            {
                var childNode = new TreeNodeViewModel
                {
                    UserId = referral.UserId,
                    FullName = referral.FullName,
                    Email = referral.Email,
                    IsActive = referral.IsActive
                };

                await BuildTreeRecursiveAsync(childNode, currentLevel + 1, maxLevels);
                node.Children.Add(childNode);
            }
        }

        /// <summary>
        /// Get all users (for admin)
        /// </summary>
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Sponsor)
                .OrderBy(u => u.RegistrationDate)
                .ToListAsync();
        }

        /// <summary>
        /// Update user active status
        /// </summary>
        public async Task<bool> UpdateUserStatusAsync(int userId, bool isActive)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Simple password hashing (for demo - use proper hashing in production)
        /// </summary>
        private string HashPassword(string password)
        {
            // Simple hash for demo - in production, use BCrypt or similar
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        /// Verify password (for demo - use proper verification in production)
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
            return hashedPassword == hash;
        }
    }
}

