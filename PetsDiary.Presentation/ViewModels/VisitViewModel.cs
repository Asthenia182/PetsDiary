using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Resources;
using System;

namespace PetsDiary.Presentation.ViewModels
{
    public class VisitViewModel : SingleViewModel
    {
        private readonly IPetsData petsData;

        public VisitViewModel(IPetsData petsData, DateTime date, int petId, int? id, string description)
            : base(petsData, petId, id)
        {
            this.petsData = petsData;
            this.Date = date;
            this.PetId = petId;
            this.Id = id;
            this.Description = description;
        }

        public override bool Save()
        {
            if (!base.Save()) return false;

            var model = new VisitModel() { Date = Date, PetId = PetId, Description = this.Description };
            petsData.AddVisit(model);

            IsDirty = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var model = new VisitModel() { Id = Id.Value, Date = Date, PetId = PetId, Description = this.Description };
            petsData.UpdateVisit(model);

            IsDirty = false;

            return true;
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                ValidateDate();
                RaisePropertyChanged();
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;

                RaisePropertyChanged();
            }
        }

        public override bool IsValid()
        {
            ValidateDate();

            return !HasErrors;
        }

        private void ValidateDate()
        {
            ClearErrors(nameof(Date));

            if (IsDirty)
            {
                if (Date == null)
                {
                    AddError(nameof(Date), ErrorMessages.ValidationErrorRequiredField);
                    return;
                }
            }            
        }

    }
}