using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace EquipApps.WorkBench.Views
{
    public class ReactiveView : ContentControl
    {
        public ReactiveView()
        {
            this.DataContextChanged += ViewLocatorControl_DataContextChanged;
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
