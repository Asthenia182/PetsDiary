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
    public class NotesViewModel : BaseViewModel
    {
        private readonly IPetsData petsData;
        private readonly IPetDescription petDescription;
        private readonly IDialogService dialogService;
        private readonly IMapper mapper;

        public DelegateCommand<int?> DeleteCommand { get; set; }
        public DelegateCommand<int?> EditCommand { get; set; }
        public DelegateCommand<int?> SaveCommand { get; set; }
        public DelegateCommand<int?> CancelCommand { get; set; }

        public NotesViewModel(IPetsData petsData, IPetDescription petDescription, IDialogService dialogService, IMapper mapper)
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
            Notes = new ObservableCollection<INoteViewModel>();
            LoadData();
        }

        private void Cancel(int? noteId)
        {
            var note = Notes.FirstOrDefault(x => x.Id == noteId);

            if (noteId.HasValue)
            {                
                note.Cancel();
            }
            else
            {
                Notes.Remove(note);
            }
        }

        private void Save(int? noteId)
        {
            var note = Notes.FirstOrDefault(x => x.Id == noteId);

            if (noteId.HasValue)
            {                
                note.Update();
            }
            else
            {
                note.Save();
            }
        }

        private void Add()
        {
            if (Notes.Any(n => !n.Id.HasValue)) return;

            var note = new NoteViewModel(petsData, mapper) { IsDirty = true, IsInEdit = true, PetId = petDescription.Id.Value };
            Notes.Insert(0, note);
        }

        protected override void Dispose(bool disposing)
        {
            AddCommand = null;
            DeleteCommand = null;
            EditCommand = null;

            Notes.ToList().ForEach(v => v.Dispose());
            Notes = null;

            base.Dispose(disposing);
        }

        public void Delete(int? noteId)
        {
            if (noteId.HasValue)
            {
                var parameters = new DialogParameters();
                parameters.Add(ParametersKeys.Message, CommonResources.WarningDelete);
                parameters.Add(ParametersKeys.Title, CommonResources.Warning);
                dialogService.ShowDialog(DialogNames.MessageDialog, parameters, (r) =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        petsData.DeleteNoteById(noteId.Value);
                        var visit = Notes.FirstOrDefault(x => x.Id == noteId.Value);
                        Notes.Remove(visit);
                    }
                }
                );
            }
        }

        public void Edit(int? noteId)
        {
            if (noteId.HasValue)
            {
                var note = Notes.FirstOrDefault(x => x.Id == noteId.Value);
                note.IsInEdit = true;
            }
        }

        private void LoadData()
        {
            Notes.Clear();

            var models = petsData.GetNotes(petDescription.Id.Value);

            foreach (var model in models)
            {
                var vm = mapper.Map(model, new NoteViewModel(petsData, mapper));
                vm.IsDirty = false;
                Notes.Add(vm);
            }
        }

        private ObservableCollection<INoteViewModel> notes;

        public ObservableCollection<INoteViewModel> Notes
        {
            get
            {
                return notes;
            }

            set
            {
                notes = value;
                RaisePropertyChanged(nameof(Notes));
            }
        }

        public DelegateCommand AddCommand { get; set; }
    }
}