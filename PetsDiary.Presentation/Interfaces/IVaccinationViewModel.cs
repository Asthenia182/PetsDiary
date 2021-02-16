using System;

namespace PetsDiary.Presentation.Interfaces
{
    public interface IVaccinationViewModel : ISingleViewModel
    {
        string Address { get; set; }
        string Name { get; set; }
        DateTime? NextShotDate { get; set; }
        DateTime? ShotDate { get; set; }
        string ShotInformation { get; set; }
    }
}