using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.WorkBench.Tools.External.Internal
{
    public class LibraryCache
    {
        private readonly Dictionary<Library, Library> _cache = new Dictionary<Library, Library>();

        public TLibrary GetOrCreate<TLibrary>() where TLibrary : Library
        {
            return null;
        }
    }
}
