using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Data;
using PetsDiary.Presentation;
using PetsDiary.Presentation.Dialogs;
using PetsDiary.Presentation.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Module
{
    public class SingleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(HomeView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AnimalView>(ViewNames.Animal);
            containerRegistry.RegisterForNavigation<HomeView>(ViewNames.Home);
            containerRegistry.RegisterForNavigation<VaccinationsView>(ViewNames.Vaccinations);
            containerRegistry.RegisterForNavigation<VisitsView>(ViewNames.Visits);
            containerRegistry.RegisterForNavigation<WeightsView>(ViewNames.Weights);

            containerRegistry.RegisterDialog<AddVaccinationDialog, AddVaccinationDialogViewModel>();
            containerRegistry.RegisterDialog<AddVisitDialog, AddVisitDialogViewModel>();
            containerRegistry.RegisterDialog<MessageDialog, MessageDialogViewModel>();
            containerRegistry.RegisterDialog<EditWeightsDialog, EditWeightsDialogViewModel>();

            containerRegistry.RegisterSingleton<IPetsData, PetsData>();
            containerRegistry.RegisterSingleton<IPetDescription, PetDescription>();
        }
    }
}