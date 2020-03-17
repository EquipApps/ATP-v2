using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.EK.Models
{
    public class CommandEtalon
    {
        public CommandEtalon(int order, string note)
        {
            this.Order = order;
            this.Note  = note;
        }

        public int Id3 { get; set; }





        public int Order    { get; set; }
        public string Note  { get; set; }










        /*
         * Отношения между таблицами.
         */

        public int CommandId { get; set; }

        public virtual Command Command { get; set; }
    }
}
