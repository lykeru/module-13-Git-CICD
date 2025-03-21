using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using person_wpf_demo.ViewModel;
using person_wpf_demo.Model;
using person_wpf_demo.Utils;
using person_wpf_demo.Data.Repositories;
using person_wpf_demo.Data.Repositories.Interfaces;
using person_wpf_demo.Services;
using person_wpf_demo.Services.Interfaces;

namespace person_wpf_demo
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<PersonsViewModel>();
            services.AddSingleton<NewPersonViewModel>();
            services.AddSingleton<NewAddressViewModel>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
            {
                BaseViewModel ViewModelFactory(Type viewModelType)
                {
                    return (BaseViewModel)serviceProvider.GetRequiredService(viewModelType);
                }
                return ViewModelFactory;
            });

            services.AddDbContext<ApplicationDbContext>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (dbContext.Database.EnsureCreated())
                {
                    dbContext.SeedData();
                }
            }

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
