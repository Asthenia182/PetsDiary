using PetsDiary.Common.Constants;
using PetsDiary.Presentation.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;

namespace PetsDiary.Presentation.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        public NavigationBarViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            this.eventAggregator
                .GetEvent<PetChangedEvent>()
                .Subscribe(UpdateName);

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string viewName)
        {
            regionManager.RequestNavigate(RegionNames.Content, viewName);
        }

        public DelegateCommand<string> NavigateCommand { get; set; }

        private void UpdateName(PetChangedEventArgs args)
        {
            PetName = args.PetName;

            if (args.IsSaved)
            {
                IsNavigationEnabled = true;
            }
            else
            {
                isNavigationEnabled = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this.eventAggregator
                .GetEvent<PetChangedEvent>()
                .Unsubscribe(UpdateName);
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