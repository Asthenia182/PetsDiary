using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Events;
using PetsDiary.Presentation.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
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
            OpenCommand = new DelegateCommand(Open);
            DeleteCommand = new DelegateCommand(Delete);
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            this.petsData = petsData;

            Pets = new ObservableCollection<PetDescription>();

            LoadData();
        }

        private void Delete()
        {
            petsData.DeleteAnimalById(SelectedItem.Id);

            Pets.Remove(SelectedItem);
        }

        private void Open()
        {
            var navigationParams = new NavigationParameters
            {
                { NavigationParameterKeys.PetId, SelectedItem.Id}
            };

            regionManager.RequestNavigate(RegionNames.Content, ViewNames.Animal, navigationParams);
        }

        private void LoadData()
        {
            Pets.Clear();

            var petModels = petsData.GetPets();

            foreach (var model in petModels)
            {
                var petDescription = new PetDescription { Id = model.Id, IsSelected = false, Name = model.Name };
                Pets.Add(petDescription);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Pets = null;
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
        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }

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

            LoadData();
        }

        private PetDescription selectedItem;

        public PetDescription SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                if (SelectedItem != null)
                {
                    SelectedItem.IsSelected = true;
                    var others = Pets.ToList().Where(x => x.Id != SelectedItem.Id);
                    others.ToList().ForEach(p => p.IsSelected = false);
                }

                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        private ObservableCollection<PetDescription> pets;

        public ObservableCollection<PetDescription> Pets
        {
            get { return pets; }
            set
            {
                pets = value;
                RaisePropertyChanged(nameof(Pets));
            }
        }

        public class PetDescription : BindableBase
        {
            private bool isSelected;

            public bool IsSelected
            {
                get { return isSelected; }
                set
                {
                    isSelected = value;
                    RaisePropertyChanged(nameof(isSelected));
                }
            }

            public string Name { get; set; }

            public int Id { get; set; }
        }
    }
}