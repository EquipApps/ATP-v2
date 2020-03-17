using DynamicData;
using DynamicData.Binding;
using EquipApps.WorkBench.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class ActionViewViewModel : ViewModelBase, IDisposable
    {
        private ReadOnlyObservableCollection<ActionItemViewModel> _items;
        private IDisposable _cleanUpConnect;

        public ActionViewViewModel(IActionService actionDescriptorService)
        {
            _cleanUpConnect = actionDescriptorService.Observable
                .Connect()
                .Transform(x => new ActionItemViewModel(x))
                .Sort(SortExpressionComparer<ActionItemViewModel>.Ascending(x => x.Number))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _items)
                .DisposeMany()
                .Subscribe();

            Filter = ReactiveCommand.Create(OnFilterd);
        }

        public ReadOnlyObservableCollection<ActionItemViewModel> Items
        {
            get
            {
                return _items;
            }
        }

        [Reactive]
        public ActionItemViewModel SelectedItem
        {
            get;
            set;
        }

        public ReactiveCommand<Unit,Unit> Filter
        {
            get;
            private set;
        }

        public void Dispose()
        {
            _cleanUpConnect.Dispose();
            _cleanUpConnect = null;
        }

        private void OnFilterd()
        {
            var selectedItem = SelectedItem;
            if (selectedItem != null)
            {
                MessageBus.Current.SendMessage
                    (selectedItem.Number.ToString(), MessageBusContracts.FilterScope);
            }
        }
    }
}
