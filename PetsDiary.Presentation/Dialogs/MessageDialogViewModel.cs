using PetsDiary.Presentation.Resources;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.IO;

namespace PetsDiary.Presentation.Dialogs
{
    public class MessageDialogViewModel : BindableBase, IDialogAware
    {
        public MessageDialogViewModel()
        {
        }

        public FileInfo FileInfo { get; set; }

        public string Title => CommonResources.Add;

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            throw new NotImplementedException();
        }

        public void OnDialogClosed()
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}