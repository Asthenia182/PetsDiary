using PetsDiary.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace PetsDiary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            throw new System.NotImplementedException(); 
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

    }
}
