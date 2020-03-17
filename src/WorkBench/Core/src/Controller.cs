﻿using EquipApps.Hardware;
using EquipApps.Mvc.Abstractions;
using EquipApps.Testing;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Mvc.ModelBinding;
using NLib.AtpNetCore.Testing.Mvc;
using System;
using System.Collections.Generic;

namespace EquipApps.WorkBench
{
    public abstract class Controller : ControllerBase, IEnableContext
    {
        [NonAction]
        public virtual void DigitalValidate(Dictionary<string, DigitalState> digitals, DigitalState etalon, params string[] digitalNames)
        {
            foreach (var digitalName in digitalNames)
            {
                DigitalValidate(digitalName, digitals[digitalName], etalon);
                digitals.Remove(digitalName);
            }
        }
        [NonAction]
        public virtual void DigitalValidate(Dictionary<string, DigitalState> digitals, DigitalState etalon, string digitalName)
        {
            DigitalValidate(digitalName, digitals[digitalName], etalon);
            digitals.Remove(digitalName);
        }
        [NonAction]
        public virtual void DigitalValidate(Dictionary<string, DigitalState> digitals, DigitalState etalon)
        {
            foreach (var pair in digitals)
            {
                DigitalValidate(pair.Key, pair.Value, etalon);
            }
        }
        [NonAction]
        public virtual void DigitalValidate(string digitalName, DigitalState digital, DigitalState etalon)
        {
            if (digital == etalon)
            {
                Logger.LogDebug
                    ("{0} - Значение {1}, Эталон {2}", digitalName, digital, etalon);
            }
            else
            {
                Logger.LogError
                    ("{0} - Значение {1}, Эталон {2}", digitalName, digital, etalon);

                ErrorOK();
            }
        }


        [NonAction]
        public virtual void ErrorOK()   
        {
            this.ControllerContext.ActionDescriptor.Result    = Result.Failed;
            this.ControllerContext.ActionDescriptor.Exception = WorkBenchException.ErrorOK;
        }

        [NonAction]
        public virtual void ErrorAAP()  
        {
            this.ControllerContext.ActionDescriptor.Result    = Result.Failed;
            this.ControllerContext.ActionDescriptor.Exception = WorkBenchException.ErrorAAP;
        }


        /// <summary>
        /// voltageValue
        /// </summary>
        /// <param name="voltageValue"></param>
        /// <param name="voltageLim"></param>
        /// <param name="voltageSize"></param>
        [NonAction]
        public virtual void VoltageValidate(double voltageValue, double voltageLim, string voltageSize = "B")
        {

        }

    }

    public abstract class Controller<T> : Controller, IModelExpected<T>
        where T : class
    {
        [BindData("")]
        public T Model { get; set; }
    }
}
