using System.Runtime.InteropServices;
using System.Windows.Markup;

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: XmlnsPrefix(@"http://workbench.equipapps.com/xaml/controls", "wb")]

[assembly: XmlnsDefinition(@"http://workbench.equipapps.com/xaml/app",      "EquipApps.WorkBench")]
[assembly: XmlnsDefinition(@"http://workbench.equipapps.com/xaml/controls", "EquipApps.WorkBench.Controls.BatteryViewer")]

[assembly: XmlnsDefinition(@"http://workbench.equipapps.com/xaml/controls", "EquipApps.WorkBench.Controls.RelayViewer")]

