using System.Windows;
using person_wpf_demo.Utils;
using person_wpf_demo.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using person_wpf_demo.Model.Interfaces;
using person_wpf_demo.Model.DAL;
using System;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.Utils.Services;

namespace person_wpf_demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProdiver;
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

            services.AddSingleton<IPersonDAL, PersonDAL>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider =>
            {
                BaseViewModel ViewModelFactory(Type viewModelType)
                {
                    return (BaseViewModel)serviceProvider.GetRequiredService(viewModelType);
                }
                return ViewModelFactory;
            });

            _serviceProdiver = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProdiver.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
