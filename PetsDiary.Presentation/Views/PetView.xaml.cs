using PetsDiary.Presentation.Interfaces;
using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for AddDialogView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class PetView : UserControl, IPetView
    {
        public PetView()
        {
            InitializeComponent();
        }
    }
}