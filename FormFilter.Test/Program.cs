using FormFilter.Test.IServices;
using FormFilter.Test.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FormFilter.Test
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
            //ÈÝÆ÷»¯
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<Form1>(); 
            services.AddSingleton<IUserService, UserService>();
            ServiceProviderHelper.serviceProvider = services.BuildServiceProvider();
            new FormRunAdapter().RunWithFilters<Form1>(form =>
            {
               Application.Run(ServiceProviderHelper.serviceProvider.GetRequiredService<Form1>());
            });
        }
    }
}