using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMApp.Models;
using MLMApp.Services;
using System.Security.Claims;

namespace MLMApp.Controllers
{
    /// <summary>
    /// Controller for admin operations
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUserService userService, ILogger<AdminController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Display admin dashboard with all users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        /// <summary>
        /// View generation tree for a specific user
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ViewGenerationTree(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userService.GetUserByUserIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var tree = await _userService.GetGenerationTreeAsync(userId, 3);
            ViewBag.UserId = userId;
            ViewBag.UserName = user.FullName;
            return View(tree);
        }

        /// <summary>
        /// Toggle user active status
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(int userId, bool isActive)
        {
            var result = await _userService.UpdateUserStatusAsync(userId, isActive);
            if (result)
            {
                TempData["SuccessMessage"] = $"User status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update user status.";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// View user details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UserDetails(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userService.GetUserByUserIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var directReferrals = await _userService.GetDirectReferralsAsync(userId);
            var totalTeamMembers = await _userService.GetTotalTeamMembersAsync(userId, 3);
            var totalIncome = await _userService.CalculateTotalIncomeAsync(userId);
            var generationLevels = await _userService.GetGenerationLevelsAsync(userId, 3);

            var viewModel = new DashboardViewModel
            {
                User = user,
                TotalDirectReferrals = directReferrals.Count,
                TotalTeamMembers = totalTeamMembers,
                TotalIncome = totalIncome,
                GenerationLevels = generationLevels
            };

            return View(viewModel);
        }

        /// <summary>
        /// Search users API endpoint
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SearchUsers(string searchTerm)
        {
            var allUsers = await _userService.GetAllUsersAsync();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                allUsers = allUsers.Where(u => 
                    u.UserId.ToLower().Contains(searchTerm) ||
                    u.FullName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.MobileNumber.Contains(searchTerm) ||
                    (u.SponsorId != null && u.SponsorId.ToLower().Contains(searchTerm))
                ).ToList();
            }

            return Json(allUsers.Select(u => new
            {
                id = u.Id,
                userId = u.UserId,
                fullName = u.FullName,
                email = u.Email,
                mobileNumber = u.MobileNumber,
                sponsorId = u.SponsorId ?? "None",
                registrationDate = u.RegistrationDate.ToString("dd/MM/yyyy"),
                isActive = u.IsActive,
                isAdmin = u.IsAdmin
            }));
        }
    }
}

