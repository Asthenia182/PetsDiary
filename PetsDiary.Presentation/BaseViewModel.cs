using Prism.Mvvm;
using Prism.Regions;

namespace PetsDiary.Presentation
{
    public class BaseViewModel: BindableBase, INavigationAware
    {
        public bool IsInEdit { get; set; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}