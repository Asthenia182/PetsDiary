using PetsDiary.Common.Constants;
using Prism.Commands;
using Prism.Regions;
using System.ComponentModel;

namespace PetsDiary.Presentation.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IPetDescription petDescription;

        public NavigationBarViewModel(IRegionManager regionManager, IPetDescription petDescription)
        {
            this.regionManager = regionManager;
            this.petDescription = petDescription;
            NavigateCommand = new DelegateCommand<string>(Navigate);

            PetName = petDescription.Name;

            IsNavigationEnabled = petDescription.Id.HasValue; 

            petDescription.PropertyChanged += PetDescription_PropertyChanged;
        }

        private void PetDescription_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(petDescription.Name))
            {
                PetName = petDescription.Name;
            }

            if (e.PropertyName == nameof(petDescription.Id))
            {
                if (petDescription.Id.HasValue)
                {
                    IsNavigationEnabled = true;
                }
                else
                {
                    isNavigationEnabled = false;
                }
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ///check navigationFrom uri?
            base.OnNavigatedTo(navigationContext);

            PetName = petDescription.Name;
            IsNavigationEnabled = false;
        }

        private void Navigate(string viewName)
        {
            regionManager.RequestNavigate(RegionNames.Content, viewName);
        }

        public DelegateCommand<string> NavigateCommand { get; set; }

        protected override void Dispose(bool disposing)
        {
            petDescription.PropertyChanged -= PetDescription_PropertyChanged;
            NavigateCommand = null;

            base.Dispose(disposing);
        }

        private string petName { get; set; }

        public string PetName
        {
            get { return petName; }
            set
            {
                petName = value;
                RaisePropertyChanged(nameof(PetName));
            }
        }

        private bool isNavigationEnabled { get; set; }

        public bool IsNavigationEnabled
        {
            get { return isNavigationEnabled; }
            set
            {
                isNavigationEnabled = value;
                RaisePropertyChanged(nameof(IsNavigationEnabled));
            }
        }
    }
}