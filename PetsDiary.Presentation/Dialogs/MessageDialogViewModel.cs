using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace PetsDiary.Presentation.Dialogs
{
    public class MessageDialogViewModel : BaseViewModel, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand<string> CloseDialogCommand =>
        _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public MessageDialogViewModel()
        {
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

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

                RaiseRequestClose(new DialogResult(result));
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
            Message = parameters.GetValue<string>(nameof(Message));
            Title = parameters.GetValue<string>(nameof(Title));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}