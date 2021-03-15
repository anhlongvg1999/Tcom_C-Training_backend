using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.Framework.Authentication
{
    public sealed class AppAuthorizeAttribute : TypeFilterAttribute
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="AppAuthorizeAttribute"/> class.  
        /// </summary>  
        /// <param name="permissions">The permissions.</param>  
        //public AppAuthorizeAttribute(params PermissionRule[] permissions)
        //  : base(typeof(AppAuthorizeExecuteAttribute))
        //{
        //    Arguments = new[] { new PermissionAuthorizationRequirement(permissions, PermissionNames.All) };
        //}
        /// <summary>  
        /// Initializes a new instance of the <see cref="AppAuthorizeAttribute"/> class.  
        /// </summary>  
        /// <param name="comparisonType">Type of the comparison.</param>  
        /// <param name="permissions">The permissions.</param>  
        public AppAuthorizeAttribute(string comparisonType = PermissionTypes.Any, params string[] permissions)
          : base(typeof(AppAuthorizeExecuteAttribute))
        {
            Arguments = new[] { new PermissionAuthorizationRequirement(permissions, comparisonType) };
        }
        /// <summary>  
        /// App authorize execution.  
        /// </summary>  
        /// <seealso cref="Attribute" />  
        /// <seealso cref="IAsyncResourceFilter" />  
        private sealed class AppAuthorizeExecuteAttribute
          : Attribute, IAsyncResourceFilter
        {
            /// <summary>  
            /// The authorization service  
            /// </summary>  
            private readonly IAuthorizationService AuthorizationService;
            /// <summary>  
            /// The required permissions  
            /// </summary>  
            private readonly PermissionAuthorizationRequirement RequiredPermissions;
            /// <summary>  
            /// Initializes a new instance of the <see cref="AppAuthorizeExecuteAttribute" /> class.  
            /// </summary>  
            /// <param name="requiredPermissions">The required permissions.</param>  
            /// <param name="authorizationService">The authorization service.</param>  
            public AppAuthorizeExecuteAttribute(
                  PermissionAuthorizationRequirement requiredPermissions,
                  IAuthorizationService authorizationService)
            {
                RequiredPermissions = requiredPermissions;
                AuthorizationService = authorizationService;
            }
            /// <summary>  
            /// Called asynchronously before the rest of the pipeline.  
            /// </summary>  
            /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ResourceExecutingContext" />.</param>  
            /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ResourceExecutionDelegate" />. Invoked to execute the next resource filter or the remainder  
            /// of the pipeline.</param>  
            /// <returns>  
            /// A <see cref="T:System.Threading.Tasks.Task" /> which will complete when the remainder of the pipeline completes.  
            /// </returns>  
            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                var authResult = await AuthorizationService.AuthorizeAsync(
                        context.HttpContext.User,
                        null,
                        new PermissionAuthorizationRequirement(RequiredPermissions.Permissions, RequiredPermissions.ComparisonMode));
                if (!authResult.Succeeded)
                {
                    //context.Result = new ChallengeResult("Bạn không có quyền dùng chức năng này!");
                    context.Result = new ChallengeResult();
                    throw new ArgumentException("ko có quyền");
                    //return;
                }
                await next?.Invoke();
            }
        }
    }
}
