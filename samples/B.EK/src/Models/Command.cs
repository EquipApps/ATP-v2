using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace B.EK.Models
{
    public class Command
    {
        public Command(string name)
        {
            this.Name = name;
        }


        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Номер ЭК.
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// Цифровая выдача Канал №1
        /// </summary>
        public string W1 { get; set; }

        /// <summary>
        /// Цифровая выдача Канал №2
        /// </summary>
        public string W2 { get; set; }

        /// <summary>
        /// Цифровая выдача Канал №3
        /// </summary>
        public string W3 { get; set; }

        /// <summary>
        /// Релейная выдача
        /// </summary>
        public string K1 { get; set; }

        /// <summary>
        /// Релейная выдача
        /// </summary>
        public string K2 { get; set; }



        /// <summary>
        /// Цифровой контроль #1
        /// </summary>
        public string R1 { get; set; }

        /// <summary>
        /// Цифровой контроль #2
        /// </summary>
        public string R2 { get; set; }

        /// <summary>
        /// Цифровой контроль #2
        /// </summary>
        public string R3 { get; set; }

        /// <summary>
        /// Релейный контрль #1
        /// </summary>
        public string F { get; set; }



        public virtual List<CommandEtalon>  CommandEtalons { get; set; } = new List<CommandEtalon>();
    }
}
