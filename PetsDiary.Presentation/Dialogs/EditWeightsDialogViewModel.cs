using PetsDiary.Presentation.Resources;
using PetsDiary.Presentation.ViewModels;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentation.Dialogs
{
    public class EditWeightsDialogViewModel : BaseViewModel, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand<string> CloseDialogCommand =>
        _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public DelegateCommand<object> DeleteCommand { get; set; }
        public string Title => CommonResources.Edit;

        public EditWeightsDialogViewModel()
        {
            DeleteCommand = new DelegateCommand<object>(Delete);
        }

        private void Delete(object obj)
        {
            Weights.Remove((WeightViewModel)obj);
        }

        private ObservableCollection<WeightViewModel> weights;

        public ObservableCollection<WeightViewModel> Weights
        {
            get { return weights; }
            set
            {
                weights = value;
                RaisePropertyChanged(nameof(Weights));
            }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        protected void CloseDialog(string parameter)
        {
            if (Weights.Any(x => !x.IsValid())) return;

            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.OK;

                var parameters = new DialogParameters();
                parameters.Add(nameof(Weights), Weights);

                RaiseRequestClose(new DialogResult(result, parameters));
            }
            else if (parameter?.ToLower() == "false")
            {
                result = ButtonResult.Cancel;

                RaiseRequestClose(new DialogResult(result));
            }
        }

        protected override void Dispose(bool disposing)
        {
            Weights.ToList().ForEach(w => w.Dispose());
            Weights = null;

            base.Dispose(disposing);
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Weights = parameters.GetValue<ObservableCollection<WeightViewModel>>(nameof(Weights));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
    }
}