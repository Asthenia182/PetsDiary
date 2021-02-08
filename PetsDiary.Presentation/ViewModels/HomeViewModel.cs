﻿using PetsDiary.Presentation.Views;
using Prism.Commands;
using Prism.Regions;

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
            regionManager.RequestNavigate("ContentRegion", "AnimalView");
        }

        public DelegateCommand AddCommand { get; private set; }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            regionManager.RegisterViewWithRegion("NavRegion", typeof(NavigationBarView));
        }
    }
}