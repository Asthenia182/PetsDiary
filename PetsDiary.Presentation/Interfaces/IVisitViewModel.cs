using System;

namespace PetsDiary.Presentation.Interfaces
{
    public interface IVisitViewModel : ISingleViewModel
    {
        DateTime Date { get; set; }
        string Description { get; set; }
    }
}