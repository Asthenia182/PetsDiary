using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Resources;
using PetsDiary.Presentation.ViewModels;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace PetsDiary.Presentation.Dialogs
{
    public class VisitDialogViewModel : BaseViewModel, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand<string> CloseDialogCommand =>
        _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public VisitDialogViewModel()
        {
        }

        private VisitViewModel visit;

        public VisitViewModel Visit
        {
            get { return visit; }
            set
            {
                visit = value;
                RaisePropertyChanged(nameof(Visit));
            }
        }

        public string Title => CommonResources.Add;

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        protected void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.OK;
                            }
            else if (parameter?.ToLower() == "false")
            {
                result = ButtonResult.Cancel;                
            }

            RaiseRequestClose(new DialogResult(result));
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Visit = parameters.GetValue<VisitViewModel>(ParametersKeys.ViewModel);
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}