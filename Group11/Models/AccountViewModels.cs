using Group11.Internationalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Group11.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter a email-adress")]
        [EmailAddress(ErrorMessage = "Please use the correct email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {   
        
        [Required(ErrorMessage = "Please enter a email-adress")]
        [EmailAddress(ErrorMessage = "Please use the correct email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please choose a nickname")]
        [RegularExpression(@"[A-Za-zåäöÅÄÖ0-9]{3,50}", ErrorMessage = "Please enter a nickname that's between 3 to 50 letter and/or numbers without spaces")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [MinLength(5, ErrorMessage = "The password need to be at least 5 character long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please write something about yourself")]
        [RegularExpression(@"^(?![\s]).+[A-Za-zåäöÅÄÖ0-9]*[A-Za-zåäöÅÄÖ0-9 ]*[A-Za-zåäöÅÄÖ0-9.]+(?![\s])$", ErrorMessage = "Please start your text with a letter or a number and end the text with a letter, number or a dot. Don't use line breaks")]
        public string Information { get; set; }

        [Display(Name = "Profile picture")]
        public byte[] UserPhoto { get; set; }
    }

    public class ChangeUserDataViewModel
    {
        [Display(ResourceType = typeof(AppResources), Name = nameof(AppResources.TitelEmail))]
        [Required(ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.ValMessageEmptyEmail))]
        [EmailAddress(ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.ValMessageEmail))]
        public string Email { get; set; }

        [Display(ResourceType = typeof(AppResources), Name = nameof(AppResources.TitelNickname))]
        [Required(ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.ValMessageEmptyNickname))]
        [RegularExpression(@"[A-Za-zåäöÅÄÖ0-9]{3,50}", ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.ValMessageNickname))]
        public string Nickname { get; set; }

        [Display(ResourceType = typeof(AppResources), Name = nameof(AppResources.TitelInformation))]
        [Required(ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.ValMessageEmptyInformation))]
        [RegularExpression(@"^(?![\s]).+[A-Za-zåäöÅÄÖ0-9]*[A-Za-zåäöÅÄÖ0-9 ]*[A-Za-zåäöÅÄÖ0-9.]+(?![\s])$", ErrorMessageResourceType = typeof(AppResources), ErrorMessageResourceName = nameof(AppResources.ValMessageInformation))]
        public string Information { get; set; }

        [Display(ResourceType = typeof(AppResources), Name = nameof(AppResources.TitelSearchable))]
        public bool Searchable { get; set; }
    }   

    public class EditProfilePictureViewModel
    {
        [Display(Name = "Profile picture")]
        public byte[] UserPhoto { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your old password")]
        [MinLength(5, ErrorMessage = "Your old password is at least 5 character long")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [MinLength(5, ErrorMessage = "The password need to be at least 5 character long")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
