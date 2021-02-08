using PetsDiary.Presentation.Interfaces;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for AddDialogView.xaml
    /// </summary>
    public partial class AnimalView : UserControl, IAnimalView
    {
        public AnimalView()
        {
            InitializeComponent();
        }
    }
}