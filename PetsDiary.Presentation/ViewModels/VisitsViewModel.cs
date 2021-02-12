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
    public class VisitsViewModel : BaseViewModel
    {
        private readonly IPetsData petsData;
        private readonly IPetDescription petDescription;
        private readonly IDialogService dialogService;

        public DelegateCommand<int?> DeleteCommand { get; set; }
        public DelegateCommand<int?> EditCommand { get; set; }

        public VisitsViewModel(IPetsData petsData, IPetDescription petDescription, IDialogService dialogService)
        {
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand<int?>(Delete);
            EditCommand = new DelegateCommand<int?>(Edit);

            this.petsData = petsData;
            this.petDescription = petDescription;
            this.dialogService = dialogService;

            Visits = new ObservableCollection<VisitModel>();

            LoadData();
        }

        private void Add()
        {
            dialogService.ShowDialog(DialogNames.AddVisitDialog, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    var visit = new VisitModel();
                    visit.Description = r.Parameters.GetValue<string>(nameof(visit.Description));
                    visit.Date = r.Parameters.GetValue<DateTime>(nameof(visit.Date));

                    visit.PetId = petDescription.Id.Value;
                    var savedModel = petsData.AddVisit(visit);

                    Visits.Add(savedModel);
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            AddCommand = null;
            DeleteCommand = null;
            Visits = null;
        }

        public void Delete(int? visitId)
        {
            if (visitId.HasValue)
            {
                var parameters = new DialogParameters();
                parameters.Add(ParametersKeys.Message, CommonResources.WarningDelete);
                parameters.Add(ParametersKeys.Title, CommonResources.Warning);
                dialogService.ShowDialog(DialogNames.MessageDialog, parameters, (r) =>
                    {
                        if (r.Result == ButtonResult.OK)
                        {
                            petsData.DeleteVisitById(visitId.Value);
                            var visit = Visits.FirstOrDefault(x => x.Id == visitId.Value);
                            Visits.Remove(visit);
                        }
                    }
                );
            }
        }

        public void Edit(int? visitId)
        {
            if (visitId.HasValue)
            {
                var visit = Visits.FirstOrDefault(x => x.Id == visitId.Value);
                var parameters = new DialogParameters();
                parameters.Add(nameof(visit.Description), visit.Description);
                parameters.Add(nameof(visit.Date), visit.Date);

                dialogService.ShowDialog(DialogNames.AddVisitDialog, parameters, r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        visit.Description = r.Parameters.GetValue<string>(nameof(visit.Description));

                        visit.Date = r.Parameters.GetValue<DateTime>(nameof(visit.Date));

                        petsData.UpdateVisit(visit);

                        Visits.Clear();
                        LoadData();
                    }
                });
            }
        }

        private void LoadData()
        {
            Visits.Clear();

            var visitModels = petsData.GetVisits(petDescription.Id.Value);

            Visits.AddRange(visitModels);
        }

        private ObservableCollection<VisitModel> visits;

        public ObservableCollection<VisitModel> Visits
        {
            get { return visits; }
            set
            {
                visits = value;
                RaisePropertyChanged(nameof(visits));
            }
        }

        public DelegateCommand AddCommand { get; set; }
    }
}