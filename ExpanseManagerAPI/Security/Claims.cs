using System.Security.Claims;

namespace ExpanseManagerAPI.Common;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(idStr, out var userId))
            return userId;

        throw new UnauthorizedAccessException("UserId claim not found or invalid.");
    }
}
