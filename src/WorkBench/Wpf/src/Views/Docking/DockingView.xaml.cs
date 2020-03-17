using EquipApps.WorkBench.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EquipApps.WorkBench.Views
{
    /// <summary>
    /// Interaction logic for DockingView.xaml
    /// </summary>
    public partial class DockingView : UserControl
    {
        public DockingView()
        {
            InitializeComponent();

            this.DataContext = Workspace.This;

            this.Loaded     += new RoutedEventHandler(DockingView_Loaded);
            this.Unloaded   += new RoutedEventHandler(DockingView_Unloaded);
        }

        private void DockingView_Loaded(object sender, RoutedEventArgs e)   
        {
            var serializer = new AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
            serializer.LayoutSerializationCallback += (s, args) =>
            {
                args.Content = args.Content;
            };

            if (File.Exists(@".\AvalonDock.config"))
                serializer.Deserialize(@".\AvalonDock.config");
        }
        private void DockingView_Unloaded(object sender, RoutedEventArgs e) 
        {
            var serializer = new AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
            serializer.Serialize(@".\AvalonDock.config");
        }
    }
}
