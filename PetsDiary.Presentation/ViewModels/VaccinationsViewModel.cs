using AutoMapper;
using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentation.ViewModels
{
    public class VaccinationsViewModel : BaseViewModel
    {
        private readonly IPetsData petsData;
        private readonly IPetDescription petDescription;
        private readonly IDialogService dialogService;
        private readonly IMapper mapper;

        public DelegateCommand<int?> DeleteCommand { get; set; }
        public DelegateCommand<int?> EditCommand { get; set; }
        public DelegateCommand<int?> SaveCommand { get; set; }
        public DelegateCommand<int?> CancelCommand { get; set; }

        public VaccinationsViewModel(IPetsData petsData, IPetDescription petDescription, IDialogService dialogService, IMapper mapper)
        {
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand<int?>(Delete);
            EditCommand = new DelegateCommand<int?>(Edit);
            SaveCommand = new DelegateCommand<int?>(Save);
            CancelCommand = new DelegateCommand<int?>(Cancel);

            this.petsData = petsData;
            this.petDescription = petDescription;
            this.dialogService = dialogService;
            this.mapper = mapper;
            Vaccinations = new ObservableCollection<IVaccinationViewModel>();

            LoadData();
        }

        public void Cancel(int? vaccinationId)
        {
            var vaccination = Vaccinations.FirstOrDefault(x => x.Id == vaccinationId);

            if (vaccinationId.HasValue)
            {
                vaccination.Cancel();
            }
            else
            {
                Vaccinations.Remove(vaccination);
            }
        }

        public void Save(int? vaccinationId)
        {
            var vaccination = Vaccinations.FirstOrDefault(x => x.Id == vaccinationId);

            if (vaccinationId.HasValue)
            {
                vaccination.Update();
            }
            else
            {
                vaccination.Save();
            }
        }

        public void Add()
        {
            if (Vaccinations.Any(v => !v.Id.HasValue)) return;

            var newVaccinaton = new VaccinationViewModel(petsData, mapper) { IsDirty = true, PetId = petDescription.Id.Value, IsInEdit = true };
            Vaccinations.Insert(0, newVaccinaton);
        }

        protected override void Dispose(bool disposing)
        {
            AddCommand = null;
            DeleteCommand = null;
            CancelCommand = null;
            EditCommand = null;
            CancelCommand = null;

            foreach (var vaccination in vaccinations)
            {
                vaccination.Dispose();
            }

            Vaccinations = null;

            base.Dispose(disposing);
        }

        public void Delete(int? vaccinationId)
        {
            if (vaccinationId.HasValue)
            {
                var parameters = new DialogParameters();
                parameters.Add(ParametersKeys.Message, CommonResources.WarningDelete);
                parameters.Add(ParametersKeys.Title, CommonResources.Warning);
                dialogService.ShowDialog(DialogNames.MessageDialog, parameters, (r) =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            petsData.DeleteVaccinationById(vaccinationId.Value);
                            var vaccination = Vaccinations.FirstOrDefault(x => x.Id == vaccinationId.Value);
                            Vaccinations.Remove(vaccination);
                        }
                    }
                );
            }
        }

        public void Edit(int? vaccinationId)
        {
            if (vaccinationId.HasValue)
            {
                var vaccination = Vaccinations.FirstOrDefault(x => x.Id == vaccinationId.Value);
                vaccination.IsInEdit = true;
            }
        }

        private void LoadData()
        {
            Vaccinations.Clear();

            var vaccinationModels = petsData.GetVaccinations(petDescription.Id.Value);

            if (vaccinationModels == null) return;

            foreach (var model in vaccinationModels)
            {
                var vm = mapper.Map(model, new VaccinationViewModel(petsData, mapper));
                vm.IsDirty = false;
                Vaccinations.Add(vm);
            }
        }

        private ObservableCollection<IVaccinationViewModel> vaccinations;

        public ObservableCollection<IVaccinationViewModel> Vaccinations
        {
            get { return vaccinations; }
            set
            {
                vaccinations = value;
                RaisePropertyChanged(nameof(vaccinations));
            }
        }

        public DelegateCommand AddCommand { get; set; }
    }
}