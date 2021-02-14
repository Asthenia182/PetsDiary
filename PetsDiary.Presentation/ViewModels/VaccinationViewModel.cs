using AutoMapper;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using System;

namespace PetsDiary.Presentation.ViewModels
{
    public class VaccinationViewModel : SingleViewModel
    {
        public VaccinationViewModel(IPetsData petsData, IMapper mapper) : base(petsData, mapper)
        {
        }

        private DateTime? shotDate;

        public DateTime? ShotDate
        {
            get { return shotDate; }
            set
            {
                shotDate = value;
                RaisePropertyChanged(nameof(ShotDate));
            }
        }

        private DateTime? nextShotDate;

        public DateTime? NextShotDate
        {
            get { return nextShotDate; }
            set
            {
                nextShotDate = value;
                RaisePropertyChanged(nameof(NextShotDate));
            }
        }

        private string name { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private string address { get; set; }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChanged(nameof(Address));
            }
        }

        private string shotInformation { get; set; }

        public string ShotInformation
        {
            get { return shotInformation; }
            set
            {
                shotInformation = value;
                RaisePropertyChanged(nameof(ShotInformation));
            }
        }

        public override bool Save()
        {
            if (!base.Save()) return false;

            var model = mapper.Map<VaccinationModel>(this);
            var savedModel = PetsData.AddVacination(model);
            Id = savedModel.Id;

            IsDirty = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var model = mapper.Map<VaccinationModel>(this);
            PetsData.UpdateVaccination(model);

            IsDirty = false;

            return true;
        }

        protected override void SaveOriginValues()
        {
            originValues.Clear();
            originValues.Add(nameof(Address), Address);
            originValues.Add(nameof(NextShotDate), NextShotDate);
            originValues.Add(nameof(ShotDate), ShotDate);
            originValues.Add(nameof(ShotInformation), ShotInformation);
            originValues.Add(nameof(Name), Name);
        }

        public override void SetValuesByOriginValues()
        {
            Address = (string)originValues[nameof(Address)];
            NextShotDate = (DateTime?)originValues[nameof(NextShotDate)];
            ShotDate = (DateTime?)originValues[nameof(ShotDate)];
            ShotInformation = (string)originValues[nameof(ShotInformation)];
            Name = (string)originValues[nameof(Name)];
        }
    }
}