using System.ComponentModel.DataAnnotations;

namespace OT.PresentationLayer.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Jméno")]
    [StringLength(100, ErrorMessage = "Jméno může mít maximálně {1} znaků")]
    public string? FirstName { get; set; }

    [Display(Name = "Příjmení")]
    [StringLength(100, ErrorMessage = "Příjmení může mít maximálně {1} znaků")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email je povinný")]
    [EmailAddress(ErrorMessage = "Neplatný formát emailu")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password must be at least {2} characters long", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Heslo")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "You must agree to the terms of service")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms of service")]
    [Display(Name = "I agree to the terms of service")]
    public bool AgreeToTerms { get; set; }
}