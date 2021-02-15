using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for VaccinationsView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class VaccinationsView : UserControl
    {
        public VaccinationsView()
        {
            InitializeComponent();
        }
    }
}