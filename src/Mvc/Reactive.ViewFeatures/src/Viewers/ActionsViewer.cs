using DynamicData;
using DynamicData.Binding;
using EquipApps.Mvc.Reactive;
using EquipApps.Mvc.Reactive.ViewFeatures.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace EquipApps.Mvc.Reactive.ViewFeatures.Viewers
{
    public class ActionsViewer : ReactiveObject, IDisposable
    {
        private ReadOnlyObservableCollection<ActionHost> _items;
        private IDisposable _cleanUp;

        public ActionsViewer(IActionService actionService)
        {
            //-- Создаем фильтр
            Filter = new ActionsViewerFilter();

            //-- Подключаемя к данным
            var sourceConnect = actionService.Observable.Connect();

            //-- Создаем счетчик
            CountTotal = new ActionsViewerCounter(sourceConnect);

            //-- Создаем счетчик
            var cleanUpConnect = sourceConnect.Filter(Filter.ObservableFilter)
                                              .Transform(x => new ActionHost(x))
                                              .Sort(SortExpressionComparer<ActionHost>.Ascending(x => x.Number))
                                              .ObserveOn(RxApp.MainThreadScheduler)
                                              .Bind(out _items)
                                              .DisposeMany()
                                              .Subscribe();

            var cleanUpBreakPointListen = MessageBusEx.ListenEnabledBreakPoint()
                .Subscribe(isEnabled => IsEnabledBreakPoint = isEnabled);

            var cleanUpCheckPointListen = MessageBusEx.ListenEnabledCheckPoint()
                .Subscribe(isEnabled => IsEnabledCheckPoint = isEnabled);

            FilterLogs = ReactiveCommand.Create(() =>
            {
                var selectedItem = SelectedItem;
                if (selectedItem != null)
                {
                    MessageBusEx.SendFilterScope(selectedItem.Number.ToString());
                }
            });

            _cleanUp = new CompositeDisposable(cleanUpConnect,
                                               cleanUpBreakPointListen,
                                               cleanUpCheckPointListen);


        }

        /// <summary>
        /// Счетчик.
        /// </summary>
        public ActionsViewerCounter CountTotal
        {
            get;
        }

        /// <summary>
        /// Фильтры.
        /// </summary>
        public ActionsViewerFilter Filter
        {
            get;
        }

        /// <summary>
        /// Разрешить / запретить BreakPoint
        /// </summary>
        [Reactive] public bool IsEnabledBreakPoint { get; set; } = true;

        /// <summary>
        /// Разрешить / запретить CheckPoint
        /// </summary>
        [Reactive] public bool IsEnabledCheckPoint { get; set; } = true;

        /// <summary>
        /// Коллекция <see cref="ActionHost"/>
        /// </summary>
        public ReadOnlyObservableCollection<ActionHost> Data
        {
            get
            {
                return _items;
            }
        }

        /// <summary>
        /// Задает или возвращает выбранный элемент
        /// </summary>
        [Reactive]
        public ActionHost SelectedItem
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ReactiveCommand<Unit, Unit> FilterLogs
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _cleanUp.Dispose();
            _cleanUp = null;
        }
    }
}
