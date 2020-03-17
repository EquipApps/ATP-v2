using EquipApps.WorkBench.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EquipApps.WorkBench.ViewModels
{
    public class LogFilterGroupVM : ViewModelBase
    {
        private readonly Dictionary<string, GroupInfo> cache;

        public LogFilterGroupVM(LogOptions options)
        {
            if(options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            cache = new Dictionary<string, GroupInfo>();

            foreach (var contextPair in options.ContextCollection)
            {
                var context  = contextPair.Key;
                var groupKey = contextPair.Value;

                if (options.GroupCollection.TryGetValue(groupKey, out GroupInfo group))
                {
                    cache.Add(context, group);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }


            ObservableFilter = this.WhenAnyValue(x => x.GroupSelected, ObservedFilter);

            Groups           = new ObservableCollection<GroupInfo>(options.GroupCollection.Values);
            GroupSelected    = Groups.FirstOrDefault();



        }


        public IObservable<Func<LogEntry, bool>> ObservableFilter { get; }

        public ObservableCollection<GroupInfo> Groups { get; }

        [Reactive]
        public GroupInfo GroupSelected { get; set; }







        private Func<LogEntry, bool> ObservedFilter(GroupInfo logGroupSelected)
        {
            return (LogEntry logEntrie) =>
            {
                if (logEntrie == null)
                    return false;

                if (logGroupSelected == null)
                    return false;

                if (logGroupSelected.ShowManyContext)
                    return true;

                var logContext = logEntrie.Context;
                if (logContext == null)
                    return logGroupSelected.ShowNullContext;

                if (cache.TryGetValue(logContext, out GroupInfo logGroupForContext))
                    return logGroupSelected.Equals(logGroupForContext);

                return false;
            };
        }
    }
}
