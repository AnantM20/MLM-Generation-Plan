using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMApp.Models;
using MLMApp.Services;
using System.Security.Claims;

namespace MLMApp.Controllers
{
    /// <summary>
    /// Controller for dashboard operations
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IUserService userService, ILogger<DashboardController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Display user dashboard
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByUserIdAsync(userIdClaim);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var directReferrals = await _userService.GetDirectReferralsAsync(userIdClaim);
            var totalTeamMembers = await _userService.GetTotalTeamMembersAsync(userIdClaim, 3);
            var totalIncome = await _userService.CalculateTotalIncomeAsync(userIdClaim);
            var generationLevels = await _userService.GetGenerationLevelsAsync(userIdClaim, 3);

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
        /// Get generation tree as JSON (for AJAX calls)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetGenerationTree()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Json(new { error = "User not found" });
            }

            var tree = await _userService.GetGenerationTreeAsync(userIdClaim, 3);
            return Json(tree);
        }

        /// <summary>
        /// Get dashboard statistics as JSON (for AJAX calls)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Json(new { error = "User not found" });
            }

            var directReferrals = await _userService.GetDirectReferralsAsync(userIdClaim);
            var totalTeamMembers = await _userService.GetTotalTeamMembersAsync(userIdClaim, 3);
            var totalIncome = await _userService.CalculateTotalIncomeAsync(userIdClaim);
            var generationLevels = await _userService.GetGenerationLevelsAsync(userIdClaim, 3);

            return Json(new
            {
                totalDirectReferrals = directReferrals.Count,
                totalTeamMembers = totalTeamMembers,
                totalIncome = totalIncome,
                generationLevels = generationLevels.Select(l => new
                {
                    level = l.Level,
                    memberCount = l.MemberCount,
                    incomePerMember = l.IncomePerMember,
                    totalIncome = l.TotalIncome
                })
            });
        }

        /// <summary>
        /// Get direct referrals as JSON (for AJAX calls)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDirectReferrals()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Json(new { error = "User not found" });
            }

            var referrals = await _userService.GetDirectReferralsAsync(userIdClaim);
            return Json(referrals.Select(r => new
            {
                userId = r.UserId,
                fullName = r.FullName,
                email = r.Email,
                mobileNumber = r.MobileNumber,
                registrationDate = r.RegistrationDate.ToString("dd/MM/yyyy"),
                isActive = r.IsActive
            }));
        }
    }
}

