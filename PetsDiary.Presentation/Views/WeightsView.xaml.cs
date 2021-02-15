using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for Weights.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class WeightsView : UserControl
    {
        public WeightsView()
        {
            InitializeComponent();            
        }      
    }   
}