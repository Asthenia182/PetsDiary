using AutoMapper;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;

namespace PetsDiary.Presentation.ViewModels
{
    public class NoteViewModel : SingleViewModel
    {
        public NoteViewModel(IPetsData petsData, IMapper mapper)
            : base(petsData, mapper)
        {
        }        

        private string note;

        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                RaisePropertyChanged(nameof(Note));
            }
        }

        public override bool Save()
        {
            if (!base.Save()) return false;

            var note = mapper.Map<NoteModel>(this);
            petsData.AddNote(note);

            IsDirty = false;
            IsInEdit = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var note = mapper.Map<NoteModel>(this);
            petsData.UpdateNote(note);

            IsDirty = false;
            IsInEdit = false;

            return true;
        }

        protected override void SaveOriginValues()
        {
            originValues.Clear();
            originValues.Add(nameof(Note), Note);
        }

        public override void SetValuesByOriginValues()
        {
            Note = (string)originValues[nameof(Note)];
        }
    }
}