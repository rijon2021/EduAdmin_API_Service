using Microsoft.EntityFrameworkCore;
using DotNet.ApplicationCore.Entities;
using DotNet.ApplicationCore.Entities.AdministrativeUnit;

namespace DotNet.Infrastructure.Persistence.Contexts
{
    public class DotNetContext : DbContext
    {
        public DotNetContext(DbContextOptions<DotNetContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<GlobalSetting> GlobalSettings { get; set; }
        public DbSet<NotificationArea> NotificationAreas { get; set; }
        public DbSet<SMSNotification> SMSNotifications { get; set; }
        public DbSet<PermissionUserRoleMap> PermissionUserRoleMaps { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<CountryDivisionMap> CountryDivisionMaps { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<DivisionDistrictMap> DivisionDistrictMaps { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<DistrictUpazilaCityCorporationMap> DistrictUpazilaCityCorporationMaps { get; set; }
        public DbSet<UpazilaCityCorporation> UpazilaCityCorporations { get; set; }
        public DbSet<UpazilaCityCorporationThanaMap> UpazilaCityCorporationThanaMaps { get; set; }
        public DbSet<Thana> Thanas { get; set; }
        public DbSet<ThanaUnionWardMap> ThanaUnionWardMaps { get; set; }
        public DbSet<UnionWard> UnionWards { get; set; }
        public DbSet<UnionWardVillageAreaMap> UnionWardVillageAreaMaps { get; set; }
        public DbSet<VillageArea> VillageAreas { get; set; }
        public DbSet<VillageAreaParaMap> VillageAreaParaMaps { get; set; }
        public DbSet<Para> Paras { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationCountryMap> OrganizationCountryMaps { get; set; }
        public DbSet<OrganizationDivisionMap> OrganizationDivisionMaps { get; set; }
        public DbSet<OrganizationDistrictMap> OrganizationDistrictMaps { get; set; }
        public DbSet<OrganizationUpazilaCityCorporationMap> OrganizationUpazilaCityCorporationMaps { get; set; }
        public DbSet<OrganizationThanaMap> OrganizationThanaMaps { get; set; }
        public DbSet<OrganizationUnionWardMap> OrganizationUnionWardMaps { get; set; }
        public DbSet<OrganizationVillageAreaMap> OrganizationVillageAreaMaps { get; set; }
        public DbSet<OrganizationParaMap> OrganizationParaMaps { get; set; }
    }
}