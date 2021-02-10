using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for VisitsView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class VisitsView : UserControl
    {
        public VisitsView()
        {
            InitializeComponent();
        }
    }
}