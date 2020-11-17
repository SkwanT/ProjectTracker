using ProjectTracker.DAL;
using ProjectTracker.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ProjectTracker
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAuthorRepository, AuthorRepository>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IReportRepository, ReportRepository>();
            container.RegisterType<IScriptRepository, ScriptRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}