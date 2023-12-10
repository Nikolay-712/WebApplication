namespace WebApp.Models.Request.Accounts;

public class RegistrationRequestModel
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}
