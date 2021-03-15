using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.Authentication
{
    /// <summary>  
    /// Permission authorization requirement.  
    /// </summary>  
    /// <seealso cref="IAuthorizationRequirement" />  
    public class PermissionAuthorizationRequirement
      : Microsoft.AspNetCore.Authorization.IAuthorizationRequirement
    {
        /// <summary>  
        /// Gets or sets the comparison mode.  
        /// </summary>  
        /// <value>  
        /// The comparison mode.  
        /// </value>  
        public string ComparisonMode { get; set; }
        /// <summary>  
        /// Initializes a new instance of the <see cref="PermissionAuthorizationRequirement"/> class.  
        /// </summary>  
        public PermissionAuthorizationRequirement()
        {
            ComparisonMode = PermissionTypes.Any;
        }
        /// <summary>  
        /// Initializes a new instance of the <see cref="PermissionAuthorizationRequirement" /> class.  
        /// </summary>  
        /// <param name="permissions">The permissions.</param>  
        /// <param name="comparisonType">Type of the comparison.</param>  
        public PermissionAuthorizationRequirement(string[] permissions, string comparisonType)
        {
            Permissions = permissions;
            ComparisonMode = comparisonType;
        }
        /// <summary>  
        /// Gets the permissions.  
        /// </summary>  
        /// <value>  
        /// The permissions.  
        /// </value>  
        public string[] Permissions { get; private set; }
    }
}
