using System;

namespace EquipApps.Mvc
{
    public static class LogOptionsEx
    {
        public static void AddContext<TContext>(this LogOptions options, string groupName)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var contextName = typeof(TContext).FullName;
            var contextNameMode = $"\"{contextName}\"";

            options.ContextCollection.Add(contextName, groupName);
            options.ContextCollection.Add(contextNameMode, groupName);
        }
    }
}
