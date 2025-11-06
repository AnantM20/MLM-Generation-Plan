using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLMApp.Models
{
    /// <summary>
    /// User model representing MLM members
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "User ID")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Sponsor ID")]
        public string? SponsorId { get; set; }

        [Display(Name = "Sponsor")]
        [ForeignKey("SponsorId")]
        public User? Sponsor { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; } = false;

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<User> Referrals { get; set; } = new List<User>();
    }

    /// <summary>
    /// View model for user registration
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(15, ErrorMessage = "Mobile Number cannot exceed 15 characters")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "Invalid mobile number format")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(50)]
        [RegularExpression(@"^REG\d+$", ErrorMessage = "Sponsor ID must be in format REG followed by numbers (e.g., REG1001)")]
        [Display(Name = "Sponsor ID (Optional)")]
        public string? SponsorId { get; set; }
    }

    /// <summary>
    /// View model for user login
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// View model for dashboard display
    /// </summary>
    public class DashboardViewModel
    {
        public User User { get; set; } = null!;
        public int TotalDirectReferrals { get; set; }
        public int TotalTeamMembers { get; set; }
        public decimal TotalIncome { get; set; }
        public List<GenerationLevelViewModel> GenerationLevels { get; set; } = new List<GenerationLevelViewModel>();
    }

    /// <summary>
    /// View model for generation level details
    /// </summary>
    public class GenerationLevelViewModel
    {
        public int Level { get; set; }
        public int MemberCount { get; set; }
        public decimal IncomePerMember { get; set; }
        public decimal TotalIncome { get; set; }
        public List<User> Members { get; set; } = new List<User>();
    }

    /// <summary>
    /// View model for generation tree node
    /// </summary>
    public class TreeNodeViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<TreeNodeViewModel> Children { get; set; } = new List<TreeNodeViewModel>();
    }
}

