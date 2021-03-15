using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSoft.Framework.Authentication
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        public PermissionAuthorizationHandler(IUserService iUserService)
        {
            // Dependency injection to get value from repository.
            _iUserService = iUserService;
        }

        public IUserService _iUserService { get; }

        protected override async System.Threading.Tasks.Task HandleRequirementAsync(
          Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext context,
          PermissionAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                throw new ArgumentException("ko có quyền");
                //return;
            }
            // Getting user Id from claims  
            string userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("ko có quyền");
               // return;
            }
            //TODO: Implement caching for this

            var userPermissions = _iUserService.GetPermission(Guid.Parse(userId)).Result.Select(x => x.Key).ToList();
            var intersect = userPermissions.Intersect(requirement.Permissions).ToList();
            var hasPermission = false;
            if (intersect != null && intersect.Count() > 0)
            {
                hasPermission = true;
            }
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
