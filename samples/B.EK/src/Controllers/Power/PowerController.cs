using B.EK.Configure;
using EquipApps.Mvc;
using EquipApps.WorkBench;
using NLib.AtpNetCore.Mvc;
using System.Threading.Tasks;

namespace B.EK.Controllers.check_Power
{
    [Area(Areas.Power)]
    [Case("10 Циклов подачи питания")]
    public class PowerController : Controller
    {
        private const int millisecondsDelay = 50;    //-- Задерка
        private int counter = 0;                     //-- Счетчик циклов


        [Step(1, "Вкл. ИП")]
        public void Action1()
        {
            //TODO: Добавить логику включения источника питания

            if(counter == 0)
            {
                Error();
            }
        }

        [Step(2, "Пауза 5с.")]
        public Task Action2()
        {
            return Task.Delay(millisecondsDelay, TestContext.TestAborted);
        }

        [Step(3, "Выкл. ИП")]
        public void Action3()
        {
            //TODO: Добавить логику вЫключения источника питания
        }

        [Step(4, "Пауза 5с.")]
        public async Task<IActionResult> Action4()
        {
            await Task.Delay(millisecondsDelay, TestContext.TestAborted);

            //-- Увеличиваем счетчик          
            counter++;

            //-- Переходим к действию!
            return (counter < 10) ? JumpTo("Action1") : Null;
        }
    }
}
