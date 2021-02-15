using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for NoteView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class NoteView : UserControl
    {
        public NoteView()
        {
            InitializeComponent();
        }
    }
}