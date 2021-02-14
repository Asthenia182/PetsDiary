using AutoMapper;
using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using Prism.Commands;
using Prism.Regions;
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
        private readonly IRegionManager regionManager;
        private readonly IMapper mapper;

        public DelegateCommand EditCommand { get; set; }

        public WeightsViewModel(IPetsData petsData, IPetDescription petDescription, IDialogService dialogService, IRegionManager regionManager, IMapper mapper)
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand(Edit);

            this.petsData = petsData;
            this.petDescription = petDescription;
            this.dialogService = dialogService;
            this.regionManager = regionManager;
            this.mapper = mapper;
            Weights = new ObservableCollection<WeightViewModel>();

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
            if (!initializedWeight.IsValid() || !CanSave) return;

            initializedWeight.Save();
   
            initializedWeight.Dispose();
            initializedWeight = null;
            InitializeWeight();

            LoadData();
        }

        private WeightViewModel initializedWeight { get; set; }

        public WeightViewModel InitializedWeight
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

        public void Delete(int? visitId)
        {
            if (visitId.HasValue)
            {
                petsData.DeleteVisitById(visitId.Value);
                var visit = Weights.FirstOrDefault(x => x.Id == visitId.Value);
                Weights.Remove(visit);
            }
        }

        public void Edit()
        {
            var parameters = new DialogParameters();
            var clonedWeights = new ObservableCollection<WeightViewModel>(Weights);

            parameters.Add(nameof(Weights), clonedWeights);

            dialogService.ShowDialog(DialogNames.EditWeightsDialog, parameters, (r) =>
             {
                 if (r.Result == ButtonResult.OK)
                 {
                     var modifiedWeights = r.Parameters.GetValue<ObservableCollection<WeightViewModel>>(nameof(Weights));

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
            var vms = new List<WeightViewModel>();
            foreach (var model in models)
            {
                var vm = mapper.Map(model, new WeightViewModel(petsData, mapper));
                vms.Add(vm);
            }
            Weights = new ObservableCollection<WeightViewModel>(vms);
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

        public DelegateCommand AddCommand { get; set; }
    }
}