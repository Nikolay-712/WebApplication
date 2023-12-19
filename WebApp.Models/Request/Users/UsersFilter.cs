namespace WebApp.Models.Request.Users;

public class UsersFilter : PaginationRequestModel
{
    public string? SearchTerm { get; set; }
}
