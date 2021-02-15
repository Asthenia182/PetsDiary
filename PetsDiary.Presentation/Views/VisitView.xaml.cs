using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for VisitView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class VisitView : UserControl
    {
        public VisitView()
        {
            InitializeComponent();
        }
    }
}