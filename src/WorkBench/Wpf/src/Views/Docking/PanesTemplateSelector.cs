using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace EquipApps.WorkBench.Views
{
	public class PanesTemplateSelector : DataTemplateSelector
	{
		public PanesTemplateSelector()
		{

		}

		public DataTemplate Reactive { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is ReactiveObject)
				return Reactive;


			return base.SelectTemplate(item, container);
		}
	}
}
