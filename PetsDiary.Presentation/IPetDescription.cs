using System.ComponentModel;

namespace PetsDiary.Presentation
{
    public interface IPetDescription : INotifyPropertyChanged
    {
        int? Id { get; set; }
        bool IsSelected { get; set; }
        string Name { get; set; }
    }
}