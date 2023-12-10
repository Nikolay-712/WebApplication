namespace WebApp.Models.Request.Accounts;

public class ConfirmEmailRequestModel
{
    public string Identifire { get; set; }

    public string Token { get; set; }
}
