using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Enums;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;

namespace PetsDiary.Presentation.ViewModels
{
    public class AnimalViewModel : BaseViewModel, IAnimalViewModel
    {
        public AnimalViewModel(IPetsData petsData, IEventAggregator eventAggregator, IPetDescription petDescription)
        {
            SaveCommand = new DelegateCommand(Save);
            EditCommand = new DelegateCommand(Edit);
            ValidateName();
            this.petsData = petsData;
            this.eventAggregator = eventAggregator;
            this.petDescription = petDescription;
        }

        protected override void OnErrorsChanged(string propertyName)
        {
            base.OnErrorsChanged(propertyName);

            CanSave = !HasErrors;
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

        public int? Id { get; set; } = null;

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

        private readonly IPetsData petsData;
        private readonly IEventAggregator eventAggregator;
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
            if (navigationContext.Parameters.ContainsKey(NavigationParameterKeys.IsNew))
            {
                IsInEdit = true;
            }
            else
            {
                // Reading saved
                var model = petsData.GetAnimalById(petDescription.Id.Value);
                SetProps(model);
                IsInEdit = false;
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

        private void Save()
        {
            var animal = new AnimalModel()
            {
                AnimalType = (int)AnimalType,
                Breed = Breed,
                Name = Name,
                Gender = (int)Gender,
                BirthDate = BirthDate,
                LastModified = DateTime.Now
            };

            if (Id.HasValue)
            {
                animal.Id = Id.Value;
                petsData.UpdateAnimal(animal);
            }
            else
            {
                petsData.AddAnimal(animal);
            }                      

            LastModified = animal.LastModified;

            IsInEdit = false;

            petDescription.Name = Name;
            petDescription.Id = animal.Id;

            //return new Task(() =>
            //{
            //    var animal = new AnimalModel()
            //    {
            //        AnimalType = (int)AnimalType,
            //        Breed = Breed,
            //        Name = Name,
            //        Gender =(int)Gender,
            //        BirthDate = BirthDate.Value,
            //        LastModified = DateTime.Now
            //    };

            //    animalData.AddAnimal(animal);
            //});
        }      
    }
}