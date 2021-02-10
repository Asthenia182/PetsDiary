using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PetsDiary.Presentation.Dialogs
{
    public class AddVisitDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand<string> CloseDialogCommand =>
        _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public AddVisitDialogViewModel()
        {
        }

        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value == null ? DateTime.Now : value;
                SetProperty(ref date, value);
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
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

                var parameters = new DialogParameters();
                parameters.Add(nameof(Description), Description);
                parameters.Add(nameof(Date), Date);

                RaiseRequestClose(new DialogResult(result, parameters));
            }
            else if (parameter?.ToLower() == "false")
            {
                result = ButtonResult.Cancel;

                RaiseRequestClose(new DialogResult(result));
            }
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Description = parameters.GetValue<string>(nameof(Description));
            Date = parameters.GetValue<DateTime>(nameof(Date));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}