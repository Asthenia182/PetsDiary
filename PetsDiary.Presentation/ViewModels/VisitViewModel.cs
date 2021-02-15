using AutoMapper;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Resources;
using System;

namespace PetsDiary.Presentation.ViewModels
{
    public class VisitViewModel : SingleViewModel
    {
        public VisitViewModel(IPetsData petsData, IMapper mapper)
            : base(petsData, mapper)
        {
        }

        public override bool Save()
        {
            if (!base.Save()) return false;

            var model = mapper.Map<VisitModel>(this);
            petsData.AddVisit(model);
            Id = model.Id;
            IsDirty = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var model = mapper.Map<VisitModel>(this);
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

        protected override void SaveOriginValues()
        {
            originValues.Clear();
            originValues.Add(nameof(Description), Description);
            originValues.Add(nameof(Date), Date);
        }

        public override void SetValuesByOriginValues()
        {
            Description = (string)originValues[nameof(Description)];
            Date = (DateTime)originValues[nameof(Date)];
        }
    }
}