using AutoMapper;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Enums;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Regions;
using System;

namespace PetsDiary.Presentation.ViewModels
{
    public class AnimalViewModel : SingleViewModel, IAnimalViewModel
    {
        public AnimalViewModel(IPetsData petsData, IPetDescription petDescription, IMapper mapper) : base(petsData, mapper)
        {
            SaveCommand = new DelegateCommand(SaveCommandExecute);
            EditCommand = new DelegateCommand(Edit);
            CancelCommand = new DelegateCommand(Cancel);

            ValidateName();
            this.petDescription = petDescription;
            this.petDescription = petDescription;
        }

        protected override void OnErrorsChanged(string propertyName)
        {
            base.OnErrorsChanged(propertyName);

            CanSave = !HasErrors;
        }

        protected override void Dispose(bool disposing)
        {
            SaveCommand = null;
            EditCommand = null;

            base.Dispose(disposing);
        }

        private bool canSave;

        public bool CanSave
        {
            get { return canSave; }
            set
            {
                canSave = value;
                RaisePropertyChanged(nameof(canSave));
            }
        }

        private void ValidateName()
        {
            ClearErrors(nameof(Name));
            if (string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), ErrorMessages.ValidationErrorRequiredField);
            else
            {
                CanSave = true;
            }
        }

        private void Edit() => IsInEdit = true;

        private string name { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ValidateName();
                RaisePropertyChanged(nameof(Name));
            }
        }

        private string breed;

        public string Breed
        {
            get { return breed; }
            set
            {
                breed = value;
                RaisePropertyChanged();
            }
        }

        private AnimalType animalType;

        public AnimalType AnimalType
        {
            get { return animalType; }
            set
            {
                animalType = value;
                RaisePropertyChanged();
            }
        }

        private Gender gender;

        public Gender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? birthDate;

        public DateTime? BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value == null ? (DateTime?)DateTime.Now : value;

                RaisePropertyChanged();
            }
        }

        private DateTime? lastModified;
        private readonly IPetDescription petDescription;

        public DateTime? LastModified
        {
            get { return lastModified; }
            set
            {
                lastModified = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Picture's file name
        /// </summary>
        public string FileName { get; set; }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters == null)
            {
                ///TODO
                throw new NullReferenceException();
            }

            // Creating new
            if (navigationContext.Parameters.ContainsKey(Constants.ParametersKeys.IsNew))
            {
                IsInEdit = true;
                IsDirty = true;
            }
            else
            {
                // Reading saved
                var model = PetsData.GetAnimalById(petDescription.Id.Value);
                SetProps(model);
                IsInEdit = false;
                IsDirty = false;
            }
        }

        private void SetProps(AnimalModel model)
        {
            AnimalType = (AnimalType)model.AnimalType;
            BirthDate = model.BirthDate;
            Name = model.Name;
            Breed = model.Breed;
            Gender = (Gender)model.Gender;
            LastModified = model.LastModified;
            Id = model.Id;
        }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand EditCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        private void SaveCommandExecute()
        {
            if (!Id.HasValue) Save(); else Update();

            IsInEdit = false;

            petDescription.Name = Name;
            petDescription.Id = Id;
        }

        public override bool Save()
        {
            if (!base.Save()) return false;

            var model = mapper.Map<AnimalModel>(this);
            PetsData.AddAnimal(model);

            Id = model.Id;
            LastModified = model.LastModified;
            IsDirty = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var model = mapper.Map<AnimalModel>(this);
            PetsData.UpdateAnimal(model);

            LastModified = model.LastModified;
            IsDirty = false;

            return true;
        }

        private void Cancel()
        {
            SetValuesByOriginValues();
            IsInEdit = false;
        }

        protected override void SaveOriginValues()
        {
            originValues.Clear();
            originValues.Add(nameof(Name), Name);
            originValues.Add(nameof(Gender), Gender);
            originValues.Add(nameof(Breed), Breed);
            originValues.Add(nameof(BirthDate), BirthDate);
            originValues.Add(nameof(AnimalType), AnimalType);
            originValues.Add(nameof(FileName), FileName);
        }

        public override void SetValuesByOriginValues()
        {
            Name = (string)originValues[nameof(Name)];
            Gender = (Gender)originValues[nameof(Gender)];
            Breed = (string)originValues[nameof(Breed)];
            BirthDate = (DateTime)originValues[nameof(BirthDate)];
            AnimalType = (AnimalType)originValues[nameof(AnimalType)];
            FileName = (string)originValues[nameof(FileName)];
        }
    }
}