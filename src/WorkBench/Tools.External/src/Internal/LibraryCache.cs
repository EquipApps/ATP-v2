using System;
using System.Collections.Generic;
using System.Reflection;

namespace EquipApps.WorkBench.Tools.External.Internal
{
    /// <summary>
    /// Singleton Pattern
    /// </summary>
    public class LibraryCache
    {
        private static Type[]   emptyTypes  = new Type[0];
        private static object[] emptyParams = new object[0];

        static LibraryCache()
        {
            Instance = new LibraryCache();
        }

        public static LibraryCache Instance { get; }



        //--------------------------------------------

        private readonly Dictionary<string, WeakReference<Library>> _cache = new Dictionary<string, WeakReference<Library>>();

        public TLibrary GetOrCreate<TLibrary>(string path) where TLibrary : Library
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            //-- Библеотека
            if (_cache.TryGetValue(path, out WeakReference<Library> weakLibrary))
            {
                //-- 
                if (weakLibrary.TryGetTarget(out Library library))
                {
                    var libraryTyped = library as TLibrary;
                    if (libraryTyped == null)
                    {
                        //-- Библеотек есть, но тип другой
                        throw new InvalidOperationException("Загружена библеотека другово типа");
                    }

                    return libraryTyped;
                }
                else
                {
                    var libraryTyped = FactoryMethod<TLibrary>(path);
                    weakLibrary.SetTarget(libraryTyped);

                    return libraryTyped;
                }
            }
            else
            {
                var libraryTyped = FactoryMethod<TLibrary>(path);
                weakLibrary = new WeakReference<Library>(libraryTyped);
                _cache.Add(path, weakLibrary);

                return libraryTyped;
            }
        }

        private static TLibrary FactoryMethod<TLibrary>(string path) where TLibrary : Library
        {
            ConstructorInfo constructor = typeof(TLibrary).GetConstructor(emptyTypes);
            if (constructor != null)
            {
                var lib = constructor.Invoke(emptyParams) as TLibrary;
                    lib.InitializeComponent(path);

                return lib;
            }
            else
            {
                throw new InvalidOperationException($"Cannot access the constructor of type: {typeof(TLibrary)}. Is the required permission granted?");
            }
        }
    }
}
