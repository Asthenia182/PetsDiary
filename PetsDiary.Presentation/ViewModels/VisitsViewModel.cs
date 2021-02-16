using AutoMapper;
using PetsDiary.Common.Constants;
using PetsDiary.Common.Interfaces;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Interfaces;
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
        private readonly IMapper mapper;

        public DelegateCommand<int?> DeleteCommand { get; set; }
        public DelegateCommand<int?> EditCommand { get; set; }

        public VisitsViewModel(IPetsData petsData, IPetDescription petDescription, IDialogService dialogService, IMapper mapper)
        {
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand<int?>(Delete);
            EditCommand = new DelegateCommand<int?>(Edit);

            this.petsData = petsData;
            this.petDescription = petDescription;
            this.dialogService = dialogService;
            this.mapper = mapper;
            Visits = new ObservableCollection<IVisitViewModel>();

            LoadData();
        }

        private void Add()
        {
            var newVisit = new VisitViewModel(petsData, mapper) { IsDirty = true, PetId = petDescription.Id.Value, Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) };

            var parameters = new DialogParameters();
            parameters.Add(ParametersKeys.ViewModel, newVisit);
            dialogService.ShowDialog(DialogNames.VisitDialog, parameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    newVisit.Save();

                    LoadData();
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            AddCommand = null;
            DeleteCommand = null;
            EditCommand = null;

            Visits.ToList().ForEach(v => v.Dispose());
            Visits = null;

            base.Dispose(disposing);
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
                parameters.Add(ParametersKeys.ViewModel, visit);
                dialogService.ShowDialog(DialogNames.VisitDialog, parameters, r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        visit.Update();

                        LoadData();
                    }
                    else
                    {
                        visit.SetValuesByOriginValues();
                    }
                });
            }
        }

        private void LoadData()
        {
            Visits.Clear();

            var models = petsData.GetVisits(petDescription.Id.Value);

            if (models == null) return;

            foreach (var model in models)
            {
                var vm = mapper.Map(model, new VisitViewModel(petsData, mapper));
                vm.IsDirty = false;
                Visits.Add(vm);
            }
        }

        private ObservableCollection<IVisitViewModel> visits;

        public ObservableCollection<IVisitViewModel> Visits
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