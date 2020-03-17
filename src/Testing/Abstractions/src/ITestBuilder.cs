using EquipApps.Testing.Features;
using System;

namespace EquipApps.Testing
{
    /// <summary>
    /// Определяет инфроструктуру создания <see cref="TestDelegate"/>
    /// </summary>
    public interface ITestBuilder
    {
        /// <summary>
        /// Возвращает <see cref="IServiceProvider"/>.
        /// </summary>
        IServiceProvider ApplicationServices { get; }

        /// <summary>
        /// Возвращает <see cref="IFeatureCollection"/>.
        /// </summary>
        IFeatureCollection ApplicationFeatures { get; }

        /// <summary>
        /// Возвращает <see cref="TestOptions"/>.
        /// </summary>
        TestOptions Options { get; }

        /// <summary>
        /// Возвращает новый <see cref="ITestBuilder"/>.
        /// </summary>    
        /// 
        /// <remarks>
        /// Используется для разветвления конвеера.
        /// </remarks>
        ITestBuilder New();

        /// <summary>
        /// Модифицирует <see cref="TestDelegate"/>
        /// </summary>       
        ITestBuilder Use(Func<TestDelegate, TestDelegate> middleware);

        /// <summary>
        /// Возвращает <see cref="TestDelegate"/>.
        /// </summary>        
        TestDelegate Build();
    }
}
