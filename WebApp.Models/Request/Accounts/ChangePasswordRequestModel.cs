namespace WebApp.Models.Request.Accounts;

public class ChangePasswordRequestModel
{
    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string Identifire { get; set; }

    public string Token { get; set; }
}
