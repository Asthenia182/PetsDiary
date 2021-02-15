using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for VaccinationView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class VaccinationView : UserControl
    {
        public VaccinationView()
        {
            InitializeComponent();
        }
    }
}