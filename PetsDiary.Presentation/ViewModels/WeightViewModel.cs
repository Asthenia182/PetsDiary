using AutoMapper;
using PetsDiary.Common.Interfaces;
using PetsDiary.Common.Models;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.Resources;
using System;
using System.Globalization;
using System.Linq;

namespace PetsDiary.Presentation.ViewModels
{
    public class WeightViewModel : SingleViewModel, IWeightViewModel
    {
        public WeightViewModel(IPetsData petsData, IMapper mapper)
            : base(petsData, mapper)
        {
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

                weightText = weight.ToString();

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

            var weight = mapper.Map<WeightModel>(this);
            var savedModel = petsData.AddWeight(weight);
            Id = savedModel.Id;

            IsDirty = false;

            return true;
        }

        public override bool Update()
        {
            if (!base.Update()) return false;

            var weight = mapper.Map<WeightModel>(this);
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

            if (result == 0)
            {
                AddError(nameof(WeightText), ErrorMessages.ValidationWeightZero);
            }
        }

        private void ValidateDate()
        {
            ClearErrors(nameof(Date));

            if (IsDirty)
            {
                if (petsData.GetWeights(PetId)
                    .Any(x => x.Date.ToString("MM/dd/yyyy") == Date.ToString("MM/dd/yyyy")))
                {
                    AddError(nameof(Date), ErrorMessages.ValidationErrorDateWeight);
                    return;
                }
            }
            else if (petsData.GetWeights(PetId)
                .Any(x => x.Date.ToString("MM/dd/yyyy") == Date.ToString("MM/dd/yyyy") && x.Id != Id))
            {
                AddError(nameof(Date), ErrorMessages.ValidationErrorDateWeight);
                return;
            }

            if (Date == null)
            {
                AddError(nameof(Date), ErrorMessages.ValidationErrorRequiredField);
            }
        }

        protected override void SaveOriginValues()
        {
            originValues.Clear();
            originValues.Add(nameof(Weight), Weight);
            originValues.Add(nameof(Date), Date);
        }

        public override void SetValuesByOriginValues()
        {
            Weight = (double)originValues[nameof(Weight)];
            Date = (DateTime)originValues[nameof(Date)];
        }
    }
}