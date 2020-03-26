using System;
using System.Collections.Generic;
using System.Text;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Модель действия
    /// </summary>
    public class Action22 : IDisposable
    {
        public Action22(long id, bool isCheckPoint, bool isBreak)
        {
            Id = id;
        }

        /// <summary>
        /// Уникальный идентификатор действия
        /// </summary>
        public long Id { get; }

        









        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
