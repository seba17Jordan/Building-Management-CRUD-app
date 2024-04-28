using BusinessLogic;
using DataAccess;
using IDataAccess;
using LogicInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServerFactory
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            services.AddScoped<ICategoryLogic, CategoryLogic>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IInvitationLogic, InvitationLogic>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();

            services.AddDbContext<DbContext, Context>(o => o.UseSqlServer(connectionString));
            return services;
        }
    }
}
