using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Resources;
using System;
using System.Globalization;
using System.Linq;

namespace PetsDiary.Presentation.ViewModels
{
    public class WeightViewModel : SingleViewModel
    {
        private readonly IPetsData petsData;

        public WeightViewModel(IPetsData petsData, DateTime date, int petId, int? id, double weight)
            : base(petsData, petId, id)
        {
            this.petsData = petsData;
            this.Date = date;
            this.PetId = petId;
            this.Id = id;
            this.Weight = weight;
            WeightText = Weight.ToString();
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

        private string weightText;

        public string WeightText
        {
            get { return weightText; }
            set
            {
                weightText = value;

                ValidateWeight();

                RaisePropertyChanged();
            }
        }

        private double weight;

        public double Weight
        {
            get { return weight; }
            set
            {
                weight = value;

                RaisePropertyChanged();
            }
        }

        public override bool IsValid()
        {
            ValidateWeight();
            ValidateDate();

            return !HasErrors;
        }

        public override bool Save()
        {
            if (!base.Save()) return false;

            var weight = new WeightModel() { Date = Date, PetId = PetId, Weight = this.Weight };
            petsData.AddWeight(weight);

            IsDirty = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var weight = new WeightModel() { Id = Id.Value, Date = Date, PetId = PetId, Weight = this.Weight };
            petsData.UpdateWeight(weight);

            IsDirty = false;

            return true;
        }

        private void ValidateWeight()
        {
            ClearErrors(nameof(WeightText));

            if (string.IsNullOrWhiteSpace(WeightText))
            {
                AddError(nameof(WeightText), ErrorMessages.ValidationErrorRequiredField);
                return;
            }

            var weightFormatted = WeightText.Replace(',', '.');
            double result;
            if (!double.TryParse(WeightText, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                AddError(nameof(WeightText), ErrorMessages.ValidationErrorWeightValueType);
            }
            else
            {
                Weight = double.Parse(weightFormatted, CultureInfo.InvariantCulture);
            }
        }

        private void ValidateDate()
        {
            ClearErrors(nameof(Date));

            if (IsDirty)
            {
                if (petsData.GetWeights(PetId).Any(x => x.Date.ToString("MM/dd/yyyy") == Date.ToString("MM/dd/yyyy")))
                {
                    AddError(nameof(Date), ErrorMessages.ValidationErrorDateWeight);
                    return;
                }
            }
            else if (petsData.GetWeights(PetId).Any(x => x.Date.ToString("MM/dd/yyyy") == Date.ToString("MM/dd/yyyy") && x.Id != Id))
            {
                AddError(nameof(Date), ErrorMessages.ValidationErrorDateWeight);
                return;
            }

            if (Date == null)
            {
                AddError(nameof(Date), ErrorMessages.ValidationErrorRequiredField);
            }
        }
    }
}