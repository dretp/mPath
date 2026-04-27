using mPath.Models.Configuration;
using mPath.Database;
using mPath.Services;
using mPath.Interface;
using mPath.Utils.Auth;
using mPath.Utils.EmployeeUitls;
using mPath.Utils.Location;
using mPath.Utils.Scheduling;
using mPath.Utils.Role;

namespace mPath.Middleware;

public static class StartupMiddleware
{
    public static void MpathRegistry(this IServiceCollection service)
    {
        
        var appConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        //app configuration
        var mPathConfig = new MpathConfiguration()
        {
            //map app settings.json to crash configuration to be used in app
            DbConnectionString = appConfig.GetSection("DBConnection").Value ?? "",
           
        };

        //Mapping Interface to Services
        service.AddSingleton<IMPathHealthPostgresDatabase, MPathHealthPostgresDatabase>();
        service.AddSingleton<IPatientService, PatientService>();
        service.AddSingleton<EmployeeUtils>();
        service.AddSingleton<IEmployeeService, EmployeeService>();
        service.AddSingleton<AuthUtil>();
        service.AddSingleton<IAuthService, AuthService>();
        service.AddSingleton<LocationUtil>();
        service.AddSingleton<SchedulingUtil>();
        service.AddSingleton<ILocationService, LocationService>();
        service.AddSingleton<ISchedulingService, SchedulingService>();
        service.AddSingleton<RoleUtil>();
        service.AddSingleton<IRoleService, RoleService>();
        
    }
}