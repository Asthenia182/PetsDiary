using System;

namespace PetsDiary.Presentation.Interfaces
{
    public interface IWeightViewModel : ISingleViewModel
    {
        DateTime Date { get; set; }
        double Weight { get; set; }
        string WeightText { get; set; }
    }
}