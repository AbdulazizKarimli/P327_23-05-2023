using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels;

public class ForgotPasswordViewModel
{
    [Required, DataType(DataType.EmailAddress), MaxLength(256)]
    public string Email { get; set; }
}