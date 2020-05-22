﻿using EquipApps.Mvc;
using EquipApps.Testing;
using EquipApps.WorkBench;
using Microsoft.Extensions.Options;
using System;

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
    [Case(1,"Исходное состояние ААП")]
    public class Etap1Controller : Controller, IEnableContext
    {
        private readonly TestOptions options;

        public Etap1Controller(IOptions<TestOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [Step(4,"Вкл. ИП1")]     
        //[OrderController("1")]
        public void Action1()
        {
            //-- 1) Определяем какой режим источника выбран.
            //switch (options.GetPowerMode<string>())
            //{
            //    case Settings.PowerMode_MIN:
            //        {
            //            break;
            //        }
            //    case Settings.PowerMode_NOM:
            //        {
            //            break;
            //        }
            //    case Settings.PowerMode_MAX:
            //        {
            //            break;
            //        }
            //    default:
            //        throw new InvalidOperationException("Выбран не поддерживаемый режим ИП");
            //}

            //this.PowerSourceTransaction(PowerSourceState.ON, "ИП1", "ИП2_+П", "ИП3_+С", "ИП4");
        }

        [Step(5,"Вкл. ИП2")]
        //[OrderController("6")]
        public void Action2()
        {
            //this.PowerSourceTransaction(PowerSourceState.OFF, "ИП1", "ИП2_+П", "ИП3_+С", "ИП4");
        }
    }
}
