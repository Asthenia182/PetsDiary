using AutoMapper;
using Microsoft.Win32;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Enums;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.Resources;
using Prism.Commands;
using Prism.Regions;
using System;
using System.IO;

namespace PetsDiary.Presentation.ViewModels
{
    public class PetViewModel : SingleViewModel, IPetViewModel
    {
        public PetViewModel(IPetsData petsData, IPetDescription petDescription, IMapper mapper) : base(petsData, mapper)
        {
            SaveCommand = new DelegateCommand(SaveCommandExecute);
            EditCommand = new DelegateCommand(Edit);
            CancelCommand = new DelegateCommand(Cancel);
            ChangeImageCommand = new DelegateCommand<EventArgs>(ChangeImage);

            ValidateName();
            this.petDescription = petDescription;
        }

        private void ChangeImage(EventArgs parameter)
        {
            if (!IsInEdit) return;

            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            bool? result = open.ShowDialog();

            if (result == true)
            {
                var filepath = open.FileName;
                //get byte array
                Image = ReadImageFile(filepath);
            }
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
            CancelCommand = null;
            ChangeImageCommand = null;

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

        private byte[] image { get; set; }

        public byte[] Image
        {
            get { return image; }
            set
            {
                image = value;

                RaisePropertyChanged();
            }
        }

        private static byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Creating new
            if (navigationContext.Parameters.ContainsKey(Constants.ParametersKeys.IsNew))
            {
                IsInEdit = true;
                IsDirty = true;
            }
            else
            {
                // Reading saved
                var model = petsData.GetPetById(petDescription.Id.Value);
                SetProps(model);
                IsInEdit = false;
                IsDirty = false;
            }
        }

        private void SetProps(PetModel model)
        {
            AnimalType = (AnimalType)model.AnimalType;
            BirthDate = model.BirthDate;
            Name = model.Name;
            Breed = model.Breed;
            Gender = (Gender)model.Gender;
            LastModified = model.LastModified;
            Id = model.Id;
            Image = model.Image;
        }

        public override bool IsValid()
        {
            ValidateName();

            return !HasErrors;
        }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand EditCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand<EventArgs> ChangeImageCommand { get; private set; }

        private void SaveCommandExecute()
        {
            bool result = !Id.HasValue ? Save() : Update();

            if (!result) return;

            IsInEdit = false;

            petDescription.Name = Name;
            petDescription.Id = Id;
        }

        public override bool Save()
        {
            if (!base.Save()) return false;

            var model = mapper.Map<PetModel>(this);
            var savedModel = petsData.AddPet(model);

            Id = savedModel.Id;
            LastModified = savedModel.LastModified;
            IsDirty = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var model = mapper.Map<PetModel>(this);
            var updatedModel = petsData.UpdatePet(model);

            LastModified = updatedModel.LastModified;
            IsDirty = false;

            return true;
        }

        protected override void SaveOriginValues()
        {
            originValues.Clear();
            originValues.Add(nameof(Name), Name);
            originValues.Add(nameof(Gender), Gender);
            originValues.Add(nameof(Breed), Breed);
            originValues.Add(nameof(BirthDate), BirthDate);
            originValues.Add(nameof(AnimalType), AnimalType);
            originValues.Add(nameof(LastModified), LastModified);
            originValues.Add(nameof(Image), Image);
        }

        public override void SetValuesByOriginValues()
        {
            Name = (string)originValues[nameof(Name)];
            Gender = (Gender)originValues[nameof(Gender)];
            Breed = (string)originValues[nameof(Breed)];
            BirthDate = (DateTime)originValues[nameof(BirthDate)];
            AnimalType = (AnimalType)originValues[nameof(AnimalType)];
            LastModified=(DateTime)originValues[nameof(LastModified)];
            Image = (byte[])originValues[nameof(Image)];
        }
    }
}