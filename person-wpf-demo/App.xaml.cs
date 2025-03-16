using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.Utils.Services;
using person_wpf_demo.ViewModel;
using person_wpf_demo.Model.Interfaces;
using person_wpf_demo.Model.DAL;
using person_wpf_demo.Model;
using person_wpf_demo.Utils;
using Microsoft.EntityFrameworkCore;
using System.IO;

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

            services.AddSingleton<IPersonDAL, PersonDAL>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, object[], BaseViewModel>>(serviceProvider =>
            {
                BaseViewModel ViewModelFactory(Type viewModelType, object[] parameters)
                {
                    if (viewModelType == typeof(NewAddressViewModel))
                    {
                        return new NewAddressViewModel(
                            serviceProvider.GetRequiredService<IPersonDAL>(),
                            serviceProvider.GetRequiredService<INavigationService>(),
                            (Person)parameters[0]);
                    }
                    return (BaseViewModel)serviceProvider.GetRequiredService(viewModelType);
                }
                return ViewModelFactory;
            });

            // Configuration de la chaîne de connexion SQLite
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DemoMVVM",
                "DemoMVVM.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            var connectionString = $"Data Source={dbPath}";

            // Enregistrement du DbContext dans le conteneur DI avec la configuration SQLite
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

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
