using System.Windows;
using System.Windows.Controls;

namespace PetsDiary.Presentation.Selectors
{
    public class ButtonDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EditButtonDataTemplate { get; set; }
        public DataTemplate SaveButtonDateTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var baseVM = (BaseViewModel)item;
            return baseVM.IsInEdit ? SaveButtonDateTemplate : EditButtonDataTemplate;
        }
    }
}