﻿using PetsDiary.Presentation.Resources;
using PetsDiary.Presentation.ViewModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace PetsDiary.Presentation.Dialogs
{
    public class EditWeightsDialogViewModel:BindableBase, IDialogAware
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