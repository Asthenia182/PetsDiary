using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
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

            Visits = new ObservableCollection<VisitViewModel>();

            LoadData();
        }

        private void Add()
        {
            dialogService.ShowDialog(DialogNames.AddVisitDialog, r =>
            {
                if (r.Result == ButtonResult.OK)
                {                    
                    var description = r.Parameters.GetValue<string>("Description");
                    var date  = r.Parameters.GetValue<DateTime>("Date");

                    var visit = new VisitViewModel(petsData, date, petDescription.Id.Value, null, description) { IsDirty = true };

                    visit.Save();                                       

                    LoadData();
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
                        visit.IsDirty = true;
                        visit.Description = r.Parameters.GetValue<string>(nameof(visit.Description));
                        visit.Date = r.Parameters.GetValue<DateTime>(nameof(visit.Date));

                        visit.Update();

                        Visits.Clear();
                        LoadData();
                    }
                });
            }
        }

        private void LoadData()
        {
            Visits.Clear();

            var models = petsData.GetVisits(petDescription.Id.Value);

            var vms = new List<VisitViewModel>();

            foreach (var model in models)
            {
                vms.Add(new VisitViewModel(petsData, model.Date, model.PetId, model.Id, model.Description) { IsDirty = false });
            }

            Visits.AddRange(vms);
        }

        private ObservableCollection<VisitViewModel> visits;

        public ObservableCollection<VisitViewModel> Visits
        {
            get
            {
                return visits;
            }

            set
            {
                visits = value;
                RaisePropertyChanged(nameof(visits));
            }
        }

        public DelegateCommand AddCommand { get; set; }
    }
}