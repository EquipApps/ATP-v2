using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.EK.Models
{
    public enum OptionPmType : int
    {
        /// <summary>
        /// Автоматический режим работы
        /// </summary>
        Nom,

        /// <summary>
        /// 10 Циклов подачи питания
        /// </summary>
        Min,

        /// <summary>
        /// Наработка
        /// </summary>
        Max,
    }
}
