namespace WebApp.Models.Request.Accounts;

public class LoginRequestModel
{
    public string Email { get; set; }

    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
