using BusinessLogic;
using DataAccess;
using Domain;
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

            services.AddScoped<IBuildingLogic, BuildingLogic>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();

            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            services.AddScoped<ICategoryLogic, CategoryLogic>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IInvitationLogic, InvitationLogic>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();

            services.AddScoped<IServiceRequestLogic, RequestLogic>();
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();

            services.AddScoped<IConstructionCompanyLogic, ConstructionCompanyLogic>();
            services.AddScoped<IConstructionCompanyRepository, ConstructionCompanyRepository>();

            services.AddScoped<IReportLogic, ReportLogic>();

            services.AddDbContext<DbContext, Context>(o => o.UseSqlServer(connectionString));
            return services;
        }
    }
}
