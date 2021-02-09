using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Events;
using PetsDiary.Presentation.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentation.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IPetsData petsData;

        public HomeViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IPetsData petsData)
        {
            AddCommand = new DelegateCommand(Add);
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            this.petsData = petsData;

            LoadData();
        }

        private void LoadData()
        {
            Animals = new ObservableCollection<AnimalModel>(petsData.GetPets());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Animals = null;
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

            var name = SelectedItem == null ? string.Empty : SelectedItem.Name;
            int? id = SelectedItem?.Id;

            var args = new PetChangedEventArgs(name, SelectedItem == null ? false : true);
            eventAggregator.GetEvent<PetChangedEvent>()
                .Publish(args);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            var navRegion = regionManager.Regions[RegionNames.Navigation];
            var navView = regionManager.Regions[RegionNames.Navigation].ActiveViews.First();
            navRegion.Remove(navView);
        }

        public AnimalModel SelectedItem { get; set; }

        public ObservableCollection<AnimalModel> Animals { get; set; }
    }
}