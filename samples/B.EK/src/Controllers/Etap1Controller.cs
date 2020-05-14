using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;

namespace B.EK.Controllers
{
    /// <summary>    
    /// <para>
    /// Контроллер исходного состояния.
    /// </para>
    /// 
    /// <para>
    /// Функциии включения и выключения питания не являются смежными.
    /// 
    /// </para>
    /// 
    /// </summary>
    [Case("Исходное состояние ААП")]
    public class Etap1Controller : Controller, IEnableContext
    {
        [Step("Вкл. ИП")]     
        [OrderController("1")]
        public void Action1()
        {
        }

        [Step("ВЫкл. ИП")]
        [OrderController("6")]
        public void Action2()
        {
        }
    }
}
