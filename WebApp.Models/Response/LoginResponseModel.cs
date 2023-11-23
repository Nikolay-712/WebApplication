namespace WebApp.Models.Response;

public class LoginResponseModel
{
    public string AccsesToken { get; set; }

    public bool IsConfirmedEmail { get; set; }

    public bool RememberMe { get; set; }
}
