using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PetsDiary.Presentation.ViewModels
{
    public class VaccinationsViewModel : BaseViewModel
    {
        private readonly IPetsData petsData;
        private readonly IPetDescription petDescription;
        private readonly IDialogService dialogService;

        public DelegateCommand<int?> DeleteCommand { get; set; }
        public DelegateCommand<int?> EditCommand { get; set; }

        public VaccinationsViewModel(IPetsData petsData, IPetDescription petDescription, IDialogService dialogService)
        {
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand<int?>(Delete);
            EditCommand = new DelegateCommand<int?>(Edit);

            this.petsData = petsData;
            this.petDescription = petDescription;
            this.dialogService = dialogService;
            Vaccinations = new ObservableCollection<VaccinationModel>();

            LoadData();
        }

        private void Add()
        {
            dialogService.ShowDialog(DialogNames.AddVaccinationDialog, new DialogParameters("message"), r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var vaccination = new VaccinationModel();
                    vaccination.Address = r.Parameters.GetValue<string>(nameof(vaccination.Address));
                    vaccination.Name = r.Parameters.GetValue<string>(nameof(vaccination.Name));
                    vaccination.ShotInformation = r.Parameters.GetValue<string>(nameof(vaccination.ShotInformation));
                    vaccination.NextShotDate = r.Parameters.GetValue<DateTime>(nameof(vaccination.NextShotDate));
                    vaccination.ShotDate = r.Parameters.GetValue<DateTime>(nameof(vaccination.ShotDate));
                    vaccination.PetId = petDescription.Id.Value;
                    var savedModel = petsData.AddVacination(vaccination);

                    Vaccinations.Add(savedModel);
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            AddCommand = null;
            DeleteCommand = null;

            foreach (var vaccination in vaccinations)
            {
              ///todo change on vm lists

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
                var parameters = new DialogParameters();
                parameters.Add(nameof(vaccination.Name), vaccination.Name);
                parameters.Add(nameof(vaccination.ShotDate), vaccination.ShotDate);
                parameters.Add(nameof(vaccination.NextShotDate), vaccination.NextShotDate);
                parameters.Add(nameof(vaccination.Address), vaccination.Address);
                parameters.Add(nameof(vaccination.ShotInformation), vaccination.ShotInformation);

                dialogService.ShowDialog(DialogNames.AddVaccinationDialog, parameters, r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        vaccination.Address = r.Parameters.GetValue<string>(nameof(vaccination.Address));
                        vaccination.Name = r.Parameters.GetValue<string>(nameof(vaccination.Name));
                        vaccination.ShotInformation = r.Parameters.GetValue<string>(nameof(vaccination.ShotInformation));
                        vaccination.NextShotDate = r.Parameters.GetValue<DateTime>(nameof(vaccination.NextShotDate));
                        vaccination.ShotDate = r.Parameters.GetValue<DateTime>(nameof(vaccination.ShotDate));

                        petsData.UpdateVaccination(vaccination);

                        Vaccinations.Clear();
                        LoadData();
                    }
                });
            }
        }

        private void LoadData()
        {
            Vaccinations.Clear();

            var vaccinationModels = petsData.GetVaccinations(petDescription.Id.Value);

            Vaccinations.AddRange(vaccinationModels);
        }

        private ObservableCollection<VaccinationModel> vaccinations;

        public ObservableCollection<VaccinationModel> Vaccinations
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