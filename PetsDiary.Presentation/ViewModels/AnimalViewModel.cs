using PetsDiary.Presentation.Constants;
using PetsDiary.Presentation.Enums;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Threading.Tasks;

namespace PetsDiary.Presentation.ViewModels
{
    public class AnimalViewModel : BaseViewModel, IAnimalViewModel
    {
        public AnimalViewModel()
        {
            SaveCommand = new DelegateCommand(async () => await Save());
            EditCommand = new DelegateCommand(Edit);
            ValidateName();
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

        public string Breed { get; set; }

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
            if (navigationContext.Parameters != null &&
                navigationContext.Parameters.ContainsKey(NavigationParameterKeys.IsInEdit))
            {
                IsInEdit = navigationContext.Parameters.GetValue<bool>(NavigationParameterKeys.IsInEdit);
            }
        }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand EditCommand { get; private set; }

        private Task Save()
        {
            return new Task(() =>
            {
            });
        }
    }
}