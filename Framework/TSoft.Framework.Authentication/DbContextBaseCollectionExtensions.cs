using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSoft.Framework.Authentication;
using TSoft.Framework.Authentication.Business.Implementations;
using TSoft.Framework.Authentication.Business.Interfaces;
using TSoft.Framework.Authentication.EmailSender;

namespace TSoft.Framework.ApiUtils
{
    public static class DbContextBaseCollectionExtensions
    {
        public static IServiceCollection RegisterDbContextBase(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {

            var migrationSoure = configuration.GetSection("MigrationDb").Value;

            //Add - Migration InitialDataContext - Context DataContext
            //Update-Database -Context DataContext
            services.AddDbContext<DataContextBase>(options => options.UseSqlServer(connectionString, o => o.MigrationsAssembly(migrationSoure)));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IPermissonService, PermissonService>();
            services.AddTransient<IEmailCodeService, EmailSenderService>();
            services.AddSingleton<IEmailConfiguration>(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            return services;
        }
    }
}
