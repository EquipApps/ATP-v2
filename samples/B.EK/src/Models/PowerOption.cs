using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.EK.Models
{
    public class PowerOption
    {
        /// <summary>
        /// Имя источника питания
        /// </summary>
        public string Name      { get; set; }

        /// <summary>
        /// Минимальное значение напряждения питания
        /// </summary>
        public double MinVoltage { get; set; }

        /// <summary>
        /// Номинальное значение напряжения питания
        /// </summary>
        public double NomVoltage { get; set; }

        /// <summary>
        /// Максимальное значение напряжения питания
        /// </summary>
        public double MaxVoltage { get; set; }
    }
}
