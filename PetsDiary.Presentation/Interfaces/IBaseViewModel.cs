using Prism.Regions;
using System;
using System.Collections;
using System.ComponentModel;

namespace PetsDiary.Presentation.Interfaces
{
    public interface IBaseViewModel :  INotifyPropertyChanged
    {
        bool HasErrors { get; }
        bool IsInEdit { get; set; }

        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        void Dispose();
        IEnumerable GetErrors(string propertyName);
        bool IsNavigationTarget(NavigationContext navigationContext);
        void OnNavigatedFrom(NavigationContext navigationContext);
        void OnNavigatedTo(NavigationContext navigationContext);
    }
}