﻿using Prism.Regions;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for NotesView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class NotesView : UserControl
    {
        public NotesView()
        {
            InitializeComponent();
        }
    }
}