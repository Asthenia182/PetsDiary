using PetsDiary.Common.Constants;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Views;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace PetsDiary.Presentation.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IRegionManager regionManager;

        public HomeViewModel(IRegionManager regionManager)
        {
            AddCommand = new DelegateCommand(Add);
            this.regionManager = regionManager;
        }

        private void Add()
        {
            var navigationParams = new NavigationParameters
            {
                { NavigationParameterKeys.IsInEdit, true}
            };

            regionManager.RequestNavigate(RegionNames.Content, ViewNames.Animal, navigationParams);
        }

        public DelegateCommand AddCommand { get; private set; }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

  
            regionManager.RegisterViewWithRegion(RegionNames.Navigation, typeof(NavigationBarView));
        }

        public AnimalModel SelectedItem { get; set; }

        public ObservableCollection<AnimalModel> Animals { get; set; }
    }
}