using Microsoft.AspNetCore.Identity;
using System.Text;

namespace WebApp.Services.Extensions;

public static class ServicesExtensions
{
    public static string DisplayIdentityResultErrorMessages(this IdentityResult identityResult)
    {
        StringBuilder errorMessage = new();

        IList<string> messages = identityResult.Errors.Select(x => x.Description).ToList();
        foreach (var message in messages)
        {
            errorMessage.AppendLine(message);
        }

        return errorMessage.ToString();
    }
}
