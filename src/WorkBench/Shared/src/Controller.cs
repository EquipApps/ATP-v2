using EquipApps.Hardware;
using EquipApps.Mvc;
using EquipApps.Mvc.Controllers;
using EquipApps.Testing;
using Microsoft.Extensions.Logging;
using NLib.AtpNetCore.Mvc;
using NLib.AtpNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace EquipApps.WorkBench
{
    public abstract class Controller : ControllerBase, IEnableContext
    {
        [NonAction]
        public virtual void Validate<TValue>(IDictionary<string, TValue> valuePairs, TValue etalon, params string[] names)
            where TValue : struct, IComparable<TValue>
        {
            foreach (var digitalName in names)
            {
                Validate(digitalName, valuePairs[digitalName], etalon);
                valuePairs.Remove(digitalName);
            }
        }
        [NonAction]
        public virtual void Validate<TValue>(IDictionary<string, TValue> valuePairs, TValue etalon, string name)
            where TValue : struct, IComparable<TValue>
        {
            Validate(name, valuePairs[name], etalon);
            valuePairs.Remove(name);
        }
        [NonAction]
        public virtual void Validate<TValue>(IDictionary<string, TValue> valuePairs, TValue etalon)
            where TValue : struct, IComparable<TValue>
        {
            foreach (var pair in valuePairs)
            {
                Validate(pair.Key, pair.Value, etalon);
            }
        }        
        [NonAction]
        public virtual void Validate<TValue>(string name, TValue value, TValue etalon)
            where TValue: struct, IComparable<TValue>
        {
            if (value.CompareTo(etalon) == 0)
            {
                Logger.LogDebug
                    ("{0} - Значение {1}, Эталон {2}", name, value, etalon);
            }
            else
            {
                Logger.LogError
                    ("{0} - Значение {1}, Эталон {2}", name, value, etalon);

                Error();
            }
        }


        [NonAction]
        public virtual void Error()   
        {
            this.ControllerContext.ActionObject.SetResult(ActionObjectResultType.Failed);
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


        #region Initialize Region

        private volatile bool isIsnitialized = false;

        /// <summary>
        /// <para> Инициализация. (Вызывается после установки контекста данных).</para>
        /// <para> Во время вызова метода, свойства (с привязкой) уже инициализированны.</para>
        /// <para></para>
        /// <para>1) Сбрасывает результат (можно отменить переопределением функции <see cref="InitializeComponent_ResetResult"/>)</para>
        /// <para>2) Выводит в протокол заголовог тестового случая (можно отменить переопределением функции <see cref="InitializeComponent_LogTestCaseInformation"/>)</para>
        /// <para>2) Выводит в протокол заголовог тестового шага (можно отменить переопределением функции <see cref="InitializeComponent_LogTestStepInformation"/>)</para>
        /// </summary>  
        /// 
        //[NonAction]
        public override void InitializeComponent()
        {
            InitializeComponent_ResetResult();
            InitializeComponent_LogTestCaseInformation();
            InitializeComponent_LogTestStepInformation();

            isIsnitialized = true;
        }

        /// <summary>
        /// Функция сброса результата. (Вызывается во время инициализации контроллера)
        /// </summary>
        protected virtual void InitializeComponent_ResetResult()
        {
            ControllerContext.ActionObject.SetResult(ActionObjectResultType.NotRun);
        }

        /// <summary>
        /// Функция логирования информации о тестовом случае.       
        /// </summary>
        protected virtual void InitializeComponent_LogTestStepInformation()
        {
            Logger.LogInformation("{Number} - {Title}",
                                  ControllerContext.ActionDescriptor.TestStep.Number,
                                  ControllerContext.ActionDescriptor.TestStep.Title);
        }

        /// <summary>
        /// Функция логирования информации о тестовом шаге.
        /// <para>Вызвается один раз для каждого экземпляра класса контроллера</para>
        /// </summary>
        protected virtual void InitializeComponent_LogTestCaseInformation()
        {
            if (isIsnitialized) return;

            Logger.LogInformation("{Number} - {Title}",
                                  ControllerContext.ActionDescriptor.TestCase.Number,
                                  ControllerContext.ActionDescriptor.TestCase.Title);
        }

        /// <summary>
        /// Заключительная функция.
        /// Вызывается после завершения действия.
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        //[NonAction]
        public override void Finally()
        {
            Finaly_PassedIfNotExecutedAndNullException();
            Finaly_LogExceptionMessage();
        }

        /// <summary>
        /// Функция формерует успешеый результат проверки. 
        /// Если результат не был изменен ранее.
        /// </summary>
        /// 
        /// <remarks>
        /// Устанавливает результат проверки как <see cref="ActionObjectResultType.Passed"/>
        /// если <see cref="ActionDescriptor.Result"/> не установлен.      
        /// </remarks>
        protected virtual void Finaly_PassedIfNotExecutedAndNullException()
        {
            if (ControllerContext.ActionObject.Result.IsEmpty)
            {
                ControllerContext.ActionObject.SetResult(ActionObjectResultType.Passed);
            }
        }

        protected virtual void Finaly_LogExceptionMessage()
        {
            var exception = ControllerContext.ActionObject.Result.Exception;
            if (exception != null)
            {
                this.Logger.LogError(exception.Message);
            }
        }

        #endregion

    }

    public abstract class Controller<T> : Controller, IModelExpected<T>
        where T : class
    {
        [BindData("")]
        public T Model { get; set; }
    }
}
