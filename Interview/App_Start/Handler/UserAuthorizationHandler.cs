using Microsoft.AspNetCore.Authorization;

namespace Interview.App_Start.Handler
{
    public class UserAuthorizationHandler : AuthorizationHandler<UserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
        {
            if (context.User.IsInRole("user"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class UserRequirement : IAuthorizationRequirement
    {
    }
}
