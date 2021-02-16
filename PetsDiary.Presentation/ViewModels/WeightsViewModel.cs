using AutoMapper;
using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Presentation.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentation.ViewModels
{
    public class WeightsViewModel : BaseViewModel
    {
        private readonly IPetsData petsData;
        private readonly IPetDescription petDescription;
        private readonly IDialogService dialogService;
        private readonly IMapper mapper;

        public DelegateCommand EditCommand { get; set; }

        public WeightsViewModel(IPetsData petsData, IPetDescription petDescription, IDialogService dialogService, IMapper mapper)
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand(Edit);

            this.petsData = petsData;
            this.petDescription = petDescription;
            this.dialogService = dialogService;
            this.mapper = mapper;
            Weights = new ObservableCollection<IWeightViewModel>();

            InitializeWeight();
            InitializedWeight.PropertyChanged += InitializedWeight_PropertyChanged;

            LoadData();
        }

        private void InitializeWeight()
        {
            InitializedWeight = new WeightViewModel(petsData, mapper)
            {
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                PetId = petDescription.Id.Value,
                Weight = 0,
                IsDirty = true
            };
        }

        private void InitializedWeight_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InitializedWeight.Weight) ||
                e.PropertyName == nameof(InitializedWeight.Date))
            {
                CanSave = !InitializedWeight.HasErrors;
            }
        }

        private void Add()
        {
            if (!CanSave) return;

            initializedWeight.Save();

            initializedWeight.Dispose();
            initializedWeight = null;
            InitializeWeight();

            LoadData();
        }

        private IWeightViewModel initializedWeight { get; set; }

        public IWeightViewModel InitializedWeight
        {
            get { return initializedWeight; }
            set
            {
                initializedWeight = value;
                RaisePropertyChanged(nameof(InitializedWeight));
            }
        }

        protected override void Dispose(bool disposing)
        {
            AddCommand = null;
            EditCommand = null;

            Weights.ToList().ForEach(w => w.Dispose());
            Weights = null;

            InitializedWeight.Dispose();
            InitializedWeight = null;

            base.Dispose(disposing);
        }
  
        public void Edit()
        {
            var parameters = new DialogParameters();
            foreach (var weight in Weights)
            {
                weight.WeightText = weight.Weight.ToString();
            }
            var clonedWeights = new ObservableCollection<IWeightViewModel>(Weights);

            parameters.Add(nameof(Weights), clonedWeights);

            dialogService.ShowDialog(DialogNames.EditWeightsDialog, parameters, (r) =>
             {
                 if (r.Result == ButtonResult.OK)
                 {
                     var modifiedWeights = r.Parameters.GetValue<ObservableCollection<IWeightViewModel>>(nameof(Weights));

                     var weightsToUpdate = Weights.Where(w => modifiedWeights.Any(mf => mf.Id == w.Id));
                     var weightsToDelete = Weights.Where(mf => !modifiedWeights.Any(w => w.Id == mf.Id));

                     foreach (var wu in weightsToUpdate)
                     {
                         wu.IsDirty = true;
                         wu.Update();
                     }

                     foreach (var wd in weightsToDelete)
                     {
                         petsData.DeleteWeightById(wd.Id.Value);
                     }

                     LoadData();
                 }
             });
        }

        private bool canSave;

        public bool CanSave
        {
            get { return canSave; }
            set
            {
                canSave = value;
                RaisePropertyChanged(nameof(canSave));
            }
        }

        private void LoadData()
        {
            Weights = null;

            var models = petsData.GetWeights(petDescription.Id.Value);

            if (models == null) return;

            var vms = new List<WeightViewModel>();
            foreach (var model in models)
            {
                var vm = mapper.Map(model, new WeightViewModel(petsData, mapper));
                vms.Add(vm);
            }
            Weights = new ObservableCollection<IWeightViewModel>(vms);
        }

        private ObservableCollection<IWeightViewModel> weights;

        public ObservableCollection<IWeightViewModel> Weights
        {
            get { return weights; }
            set
            {
                weights = value;
                RaisePropertyChanged(nameof(Weights));
            }
        }

        public DelegateCommand AddCommand { get; set; }
    }
}