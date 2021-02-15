using AutoMapper;
using PetsDiary.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace PetsDiary.Presentation.ViewModels
{
    public abstract class SingleViewModel : BaseViewModel
    {
        public SingleViewModel(IPetsData petsData, IMapper mapper)
        {
            this.petsData = petsData;
            this.mapper = mapper;
            originValues = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> originValues;
        protected readonly IMapper mapper;

        public virtual bool IsValid() => true;

        protected override void Dispose(bool disposing)
        {
            originValues = null;

            base.Dispose(disposing);
        }

        private bool isDirty;

        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                isDirty = value;

                if (!IsDirty)
                    SaveOriginValues();

                    RaisePropertyChanged(nameof(IsDirty));
            }
        }

        protected abstract void SaveOriginValues();

        public abstract void SetValuesByOriginValues();

        protected IPetsData petsData { get; }

        public int PetId { get; set; }

        public int? Id { get; set; }


        public virtual bool Save()
        {
            if (Id.HasValue)
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
            if (!Id.HasValue)
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