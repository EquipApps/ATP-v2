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
    public class ActionViewModel : ViewModelBase, IDisposable
    {
        private ReadOnlyObservableCollection<ActionItem> _items;
        private IDisposable _cleanUpConnect;

        public ActionViewModel(IActionService actionDescriptorService)
        {
            _cleanUpConnect = actionDescriptorService.Observable
                .Connect()
                .Transform(x => new ActionItem(x))
                .Sort(SortExpressionComparer<ActionItem>.Ascending(x => x.Number))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _items)
                .DisposeMany()
                .Subscribe();

            Filter = ReactiveCommand.Create(OnFilterd);
        }

        /// <summary>
        /// Разрешить / запретить BreakPoint
        /// </summary>
        [Reactive] public bool IsEnabledBreakPoint { get; set; } = true;

        /// <summary>
        /// Разрешить / запретить CheckPoint
        /// </summary>
        [Reactive] public bool IsEnabledCheckPoint { get; set; } = true;




        public ReadOnlyObservableCollection<ActionItem> Items
        {
            get
            {
                return _items;
            }
        }

        [Reactive]
        public ActionItem SelectedItem
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
