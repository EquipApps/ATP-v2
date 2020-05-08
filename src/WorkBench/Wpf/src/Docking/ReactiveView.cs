using ReactiveUI;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace EquipApps.WorkBench.Docking
{
    public class ReactiveView : ContentControl
    {
        public ReactiveView()
        {
            DataContextChanged += ViewLocatorControl_DataContextChanged;
        }

        private void ViewLocatorControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var datacotext = e.NewValue;
            if (datacotext == null)
            {
                return;
            }
            var view = ViewLocator.Current.ResolveView(datacotext);
            if (view == null)
            {
                return;
            }

            if (view.ViewModel == null)
            {
                view.ViewModel = datacotext;
            }

            Content = view;
        }
    }
}
