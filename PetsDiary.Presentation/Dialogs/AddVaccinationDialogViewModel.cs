using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PetsDiary.Presentation.Dialogs
{
    public class AddVaccinationDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand<string> CloseDialogCommand =>
        _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public AddVaccinationDialogViewModel()
        {
        }

        private DateTime shotDate;

        public DateTime ShotDate
        {
            get { return shotDate; }
            set 
            {
                shotDate = value == null ? DateTime.Now : value;
                SetProperty(ref shotDate, value);
            }
        }

        private DateTime nextShotDate;

        public DateTime NextShotDate
        {
            get { return nextShotDate; }
            set 
            {
                nextShotDate = value == null ? DateTime.Now : value;
                SetProperty(ref nextShotDate, value);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string shotInformation;

        public string ShotInformation
        {
            get { return shotInformation; }
            set { SetProperty(ref shotInformation, value); }
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
                parameters.Add(nameof(Name), Name);
                parameters.Add(nameof(ShotDate), ShotDate);
                parameters.Add(nameof(NextShotDate), NextShotDate);
                parameters.Add(nameof(Address), Address);
                parameters.Add(nameof(ShotInformation), ShotInformation);

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
            //throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Address = parameters.GetValue<string>(nameof(Address));
            Name = parameters.GetValue<string>(nameof(Name));
            ShotInformation = parameters.GetValue<string>(nameof(ShotInformation));
            NextShotDate = parameters.GetValue<DateTime>(nameof(NextShotDate));
            ShotDate = parameters.GetValue<DateTime>(nameof(ShotDate));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}