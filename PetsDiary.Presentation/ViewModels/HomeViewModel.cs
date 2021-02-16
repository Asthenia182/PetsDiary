using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Resources;
using PetsDiary.Presentation.Views;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentation.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IPetsData petsData;
        private readonly IPetDescription petDescription;
        private readonly IDialogService dialogService;

        public HomeViewModel(IRegionManager regionManager, IPetsData petsData, IPetDescription petDescription, IDialogService dialogService)
        {
            AddCommand = new DelegateCommand(Add);
            OpenCommand = new DelegateCommand(Open);
            DeleteCommand = new DelegateCommand(Delete);
            this.regionManager = regionManager;
            this.petsData = petsData;
            this.petDescription = petDescription;
            this.dialogService = dialogService;
            Pets = new ObservableCollection<IPetDescription>();

            LoadData();
        }

        private void Delete()
        {
            var parameters = new DialogParameters();
            parameters.Add(ParametersKeys.Message, CommonResources.WarningDelete);
            parameters.Add(ParametersKeys.Title, CommonResources.Warning);
            dialogService.ShowDialog(DialogNames.MessageDialog, parameters, (r) =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    petsData.DeletePetById(SelectedItem.Id.Value);

                    Pets.Remove(SelectedItem);
                }
            }
            );
        }

        private void Open()
        {
            var navigationParams = new NavigationParameters
            {
                { ParametersKeys.PetId, SelectedItem.Id}
            };

            petDescription.Id = SelectedItem.Id;
            petDescription.Name = SelectedItem.Name;
            petDescription.IsSelected = SelectedItem.IsSelected;

            regionManager.RequestNavigate(RegionNames.Content, ViewNames.Pet, navigationParams);
        }

        private void LoadData()
        {
            Pets.Clear();

            var petModels = petsData.GetPets();

            foreach (var model in petModels)
            {
                var petDescription = new PetDescription { Id = model.Id, IsSelected = false, Name = model.Name, Image=model.Image };
                Pets.Add(petDescription);
            }
        }

        protected override void Dispose(bool disposing)
        {
            AddCommand = null;
            OpenCommand = null;
            DeleteCommand = null;
            Pets = null;

            base.Dispose(disposing);
        }

        private void Add()
        {
            var navigationParams = new NavigationParameters
            {
                { ParametersKeys.IsNew, true}
            };

            regionManager.RequestNavigate(RegionNames.Content, ViewNames.Pet, navigationParams);
        }

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            regionManager.RegisterViewWithRegion(RegionNames.Navigation, typeof(NavigationBarView));
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            var navRegion = regionManager.Regions[RegionNames.Navigation];
            if (navRegion != null)
            {
                var navView = regionManager.Regions[RegionNames.Navigation].ActiveViews.First();
                navRegion.Remove(navView);
            }

            petDescription.Id = null;
            petDescription.IsSelected = false;
            petDescription.Name = string.Empty;

            LoadData();
        }

        private IPetDescription selectedItem;

        public IPetDescription SelectedItem
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

        private ObservableCollection<IPetDescription> pets;

        public ObservableCollection<IPetDescription> Pets
        {
            get { return pets; }
            set
            {
                pets = value;
                RaisePropertyChanged(nameof(Pets));
            }
        }
    }
}