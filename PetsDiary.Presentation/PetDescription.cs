using Prism.Mvvm;

namespace PetsDiary.Presentation
{
    public class PetDescription : BindableBase, IPetDescription
    {
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged(nameof(isSelected));
            }
        }

        private string name { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private int? id { get; set; }

        public int? Id
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged(nameof(id));
            }
        }
    }
}