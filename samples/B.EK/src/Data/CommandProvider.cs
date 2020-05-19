using B.EK.Models;
using EquipApps.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.EK.Data
{
    public class CommandProvider : ModelProvider<Command>
    {
        public override IReadOnlyList<Command> Provide()
        {
            List<Command> ddd = new List<Command>();

            for (int i = 1; i < 17; i++)
            {
                ddd.Add(new Command("ЭК" + i)
                {
                    W1 = $"W{i}.1",
                    W2 = $"W{i}.2",
                    W3 = $"W{i}.3",

                    
                    K1 = "K24", //TODO: Сгенирировать реле
                    K2 = "K25",

                    R1 = $"R{i}.1",
                    R2 = $"R{i}.2",
                    R3 = $"R{i}.3",

                    F = $"F{i}",

                    CommandEtalons = new List<CommandEtalon>()
                    {
                        new CommandEtalon(1,"Канал - 1"      ),
                        new CommandEtalon(2,"Канал - 2"      ),
                        new CommandEtalon(3,"Канал - 3"      ),
                        new CommandEtalon(4,"Канал - 1,2"    ),
                        new CommandEtalon(5,"Канал - 2,3"    ),
                        new CommandEtalon(6,"Канал - 1,3"    ),
                        new CommandEtalon(7,"Канал - 1,2,3"  ),
                    }
                }); ; ;
            }

            return ddd;
        }
    }
}
