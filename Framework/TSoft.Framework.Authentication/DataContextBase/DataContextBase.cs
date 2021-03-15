using Microsoft.EntityFrameworkCore;

namespace TSoft.Framework.Authentication
{
    public class DataContextBase : DbContext
    {
        public DataContextBase(DbContextOptions<DataContextBase> options)
        : base(options)
        {
        }

        protected DataContextBase(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<User> AppUsers { get; set; }
        public DbSet<Role> AppRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermisson> RolePermissons { get; set; }
        public DbSet<Permisson> Permissons { get; set; }
    }
}
