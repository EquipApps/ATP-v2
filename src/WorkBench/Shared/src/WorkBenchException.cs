using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipApps.WorkBench
{
    public class WorkBenchException : Exception
    {
        private WorkBenchException(string message) : base(message)
        {
        }

        public static WorkBenchException ErrorOK    { get; } = new WorkBenchException("Ошибка ОК");
        public static WorkBenchException ErrorAAP   { get; } = new WorkBenchException("Ошибка ААП");
    }
}
