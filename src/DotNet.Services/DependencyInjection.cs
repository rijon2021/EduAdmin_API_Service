using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotNet.Infrastructure.Persistence.Contexts;
using System.Diagnostics.CodeAnalysis;
using DotNet.Services.Repositories.Common;
using DotNet.Services.Repositories.Infrastructure;
using DotNet.Services.Services.Infrastructure;
using DotNet.Services.Repositories.Interfaces.Common;
using DotNet.Services.Services.Interfaces.Common;
using DotNet.Services.Services.Common;
using DotNet.Services.Services.Common.AdministrativeUnit;
using DotNet.Services.Services.Interfaces.Common.AdministrativeUnit;
using DotNet.Services.Repositories.Interfaces.Common.AdministrativeUnit;
using DotNet.Services.Repositories.Common.AdministrativeUnit;

namespace DotNet.Services
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DotNetContext>(options => options.UseSqlServer(defaultConnectionString));
            services.AddScoped<DbContext, DotNetContext>();

            DependencyInjection.AddRepositories(services);
            DependencyInjection.AddServices(services);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //configuration.UseMiddleware<ErrorHandlerMiddleware>();

            var serviceProvider = services.BuildServiceProvider();
            try
            {
                var dbContext = serviceProvider.GetRequiredService<DotNetContext>();
                dbContext.Database.Migrate();
            }
            catch
            {
            }
            return services;
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            //services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IGlobalSettingRepository, GlobalSettingRepository>();
            services.AddScoped<INotificationAreaRepository, NotificationAreaRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPermissionUserRoleMapRepository, PermissionUserRoleMapRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDivisionRepository, DivisionRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IUpazilaCityCorporationRepository, UpazilaCityCorporationRepository>();
            services.AddScoped<IThanaRepository, ThanaRepository>();
            services.AddScoped<IUnionWardRepository, UnionWardRepository>();
            services.AddScoped<IVillageAreaRepository, VillageAreaRepository>();
            services.AddScoped<IParaRepository, ParaRepository>();

        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IGlobalSettingService, GlobalSettingService>();
            services.AddScoped<INotificationAreaService, NotificationAreaService>();
            services.AddScoped<IOrganizaionService, OrganizaionService>();
            services.AddScoped<IPermissionUserRoleMapService, PermissionUserRoleMapService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IDivisionService, DivisionService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IUpazilaCityCorporationService, UpazilaCityCorporationService>();
            services.AddScoped<IThanaService, ThanaService>();
            services.AddScoped<IUnionWardService, UnionWardService>();
            services.AddScoped<IVillageAreaService, VillageAreaService>();
            services.AddScoped<IParaService, ParaService>();
            services.AddScoped<IOrganizationAdministrativeUnitMapService, OrganizationAdministrativeUnitMapService>();

        }
    }
}