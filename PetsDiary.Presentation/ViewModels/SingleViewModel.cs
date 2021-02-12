using PetsDiary.Common.Interfaces;
using System;

namespace PetsDiary.Presentation.ViewModels
{
    public abstract class SingleViewModel : BaseViewModel
    {
        public SingleViewModel(IPetsData petsData, int petId, int? id)
        {
            PetsData = petsData;
            this.PetId = petId;
            this.Id = id;
        }

        public virtual bool IsValid() => true;

        public bool IsDirty { get; set; }

        protected IPetsData PetsData { get; }

        public int PetId { get; set; }

        public int? Id { get; set; }

        public virtual bool Save()
        {
            if (!IsDirty)
            {
                throw new ArgumentException("Should be updated not saved");
            }
            else if (!IsValid())
            {
                return false;
            }

            return true;
        }

        public virtual bool Update()
        {
            if (!IsDirty || !Id.HasValue)
            {
                throw new ArgumentException("Cannot update unsaved object");
            }
            else if (!IsValid())
            {
                return false;
            }

            return true;
        }
    }
}