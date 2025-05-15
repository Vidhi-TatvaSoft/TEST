using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class RegistrationViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;

     [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;

[Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
    public string Password {get ; set; } = null!;
}
