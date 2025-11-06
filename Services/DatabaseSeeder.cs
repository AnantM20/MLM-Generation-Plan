using Microsoft.EntityFrameworkCore;
using MLMApp.Data;
using MLMApp.Models;

namespace MLMApp.Services
{
    /// <summary>
    /// Service to seed initial database data
    /// </summary>
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(ApplicationDbContext context, ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Seed initial data if database is empty, or update admin referrals if needed
        /// </summary>
        public async Task SeedAsync()
        {
            try
            {
                var hasData = await _context.Users.AnyAsync();
                
                // If database has data, ensure admin has referrals
                if (hasData)
                {
                    await EnsureAdminHasReferralsAsync();
                    _logger.LogInformation("Database already contains data. Admin referrals checked/updated.");
                    return;
                }

                _logger.LogInformation("Starting database seeding...");

                var users = new List<User>();

                // Admin User
                users.Add(new User
                {
                    FullName = "Admin User",
                    Email = "admin@mlm.com",
                    MobileNumber = "9999999999",
                    UserId = "REG1000",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Admin@123")),
                    SponsorId = null,
                    IsActive = true,
                    IsAdmin = true,
                    RegistrationDate = DateTime.Now
                });

                // Root User (Level 0) - Sponsored by Admin
                users.Add(new User
                {
                    FullName = "John Doe",
                    Email = "john.doe@example.com",
                    MobileNumber = "9876543210",
                    UserId = "REG1001",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1000", // Admin is the sponsor
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-15)
                });

                // Additional Direct Referrals for Admin (REG1000)
                users.Add(new User
                {
                    FullName = "Sarah Connor",
                    Email = "sarah.connor@example.com",
                    MobileNumber = "9876543225",
                    UserId = "REG1016",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1000", // Admin is the sponsor
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-16)
                });

                users.Add(new User
                {
                    FullName = "Mike Tyson",
                    Email = "mike.tyson@example.com",
                    MobileNumber = "9876543226",
                    UserId = "REG1017",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1000", // Admin is the sponsor
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-17)
                });

                // Level 1 Users (Direct referrals of REG1001)
                users.Add(new User
                {
                    FullName = "Alice Smith",
                    Email = "alice.smith@example.com",
                    MobileNumber = "9876543211",
                    UserId = "REG1002",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1001",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-14)
                });

                users.Add(new User
                {
                    FullName = "Bob Johnson",
                    Email = "bob.johnson@example.com",
                    MobileNumber = "9876543212",
                    UserId = "REG1003",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1001",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-13)
                });

                users.Add(new User
                {
                    FullName = "Carol Williams",
                    Email = "carol.williams@example.com",
                    MobileNumber = "9876543213",
                    UserId = "REG1004",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1001",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-12)
                });

                // Level 2 Users
                users.Add(new User
                {
                    FullName = "David Brown",
                    Email = "david.brown@example.com",
                    MobileNumber = "9876543214",
                    UserId = "REG1005",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1002",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-11)
                });

                users.Add(new User
                {
                    FullName = "Emma Davis",
                    Email = "emma.davis@example.com",
                    MobileNumber = "9876543215",
                    UserId = "REG1006",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1002",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-10)
                });

                users.Add(new User
                {
                    FullName = "Frank Miller",
                    Email = "frank.miller@example.com",
                    MobileNumber = "9876543216",
                    UserId = "REG1007",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1003",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-9)
                });

                users.Add(new User
                {
                    FullName = "Grace Wilson",
                    Email = "grace.wilson@example.com",
                    MobileNumber = "9876543217",
                    UserId = "REG1008",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1003",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-8)
                });

                users.Add(new User
                {
                    FullName = "Henry Moore",
                    Email = "henry.moore@example.com",
                    MobileNumber = "9876543218",
                    UserId = "REG1009",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1004",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-7)
                });

                // Level 3 Users
                users.Add(new User
                {
                    FullName = "Ivy Taylor",
                    Email = "ivy.taylor@example.com",
                    MobileNumber = "9876543219",
                    UserId = "REG1010",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1005",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-6)
                });

                users.Add(new User
                {
                    FullName = "Jack Anderson",
                    Email = "jack.anderson@example.com",
                    MobileNumber = "9876543220",
                    UserId = "REG1011",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1005",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-5)
                });

                users.Add(new User
                {
                    FullName = "Kate Thomas",
                    Email = "kate.thomas@example.com",
                    MobileNumber = "9876543221",
                    UserId = "REG1012",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1006",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-4)
                });

                users.Add(new User
                {
                    FullName = "Leo Jackson",
                    Email = "leo.jackson@example.com",
                    MobileNumber = "9876543222",
                    UserId = "REG1013",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1007",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-3)
                });

                users.Add(new User
                {
                    FullName = "Mia White",
                    Email = "mia.white@example.com",
                    MobileNumber = "9876543223",
                    UserId = "REG1014",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1008",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-2)
                });

                users.Add(new User
                {
                    FullName = "Noah Harris",
                    Email = "noah.harris@example.com",
                    MobileNumber = "9876543224",
                    UserId = "REG1015",
                    Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                    SponsorId = "REG1009",
                    IsActive = true,
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-1)
                });

                _context.Users.AddRange(users);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully seeded {users.Count} users to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding database");
                throw;
            }
        }

        /// <summary>
        /// Ensure admin user has direct referrals
        /// </summary>
        private async Task EnsureAdminHasReferralsAsync()
        {
            try
            {
                var admin = await _context.Users.FirstOrDefaultAsync(u => u.UserId == "REG1000");
                if (admin == null) return;

                // Update REG1001 to be sponsored by Admin if it's not already
                var johnDoe = await _context.Users.FirstOrDefaultAsync(u => u.UserId == "REG1001");
                if (johnDoe != null && string.IsNullOrEmpty(johnDoe.SponsorId))
                {
                    johnDoe.SponsorId = "REG1000";
                    _context.Users.Update(johnDoe);
                }

                // Add Sarah Connor if she doesn't exist
                if (!await _context.Users.AnyAsync(u => u.UserId == "REG1016"))
                {
                    _context.Users.Add(new User
                    {
                        FullName = "Sarah Connor",
                        Email = "sarah.connor@example.com",
                        MobileNumber = "9876543225",
                        UserId = "REG1016",
                        Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                        SponsorId = "REG1000",
                        IsActive = true,
                        IsAdmin = false,
                        RegistrationDate = DateTime.Now.AddDays(-16)
                    });
                }

                // Add Mike Tyson if he doesn't exist
                if (!await _context.Users.AnyAsync(u => u.UserId == "REG1017"))
                {
                    _context.Users.Add(new User
                    {
                        FullName = "Mike Tyson",
                        Email = "mike.tyson@example.com",
                        MobileNumber = "9876543226",
                        UserId = "REG1017",
                        Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test@123")),
                        SponsorId = "REG1000",
                        IsActive = true,
                        IsAdmin = false,
                        RegistrationDate = DateTime.Now.AddDays(-17)
                    });
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Admin referrals updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating admin referrals");
                // Don't throw - this is not critical
            }
        }
    }
}

