namespace EquipApps.Testing
{
    public static class SettingEx
    {
        private const string WorkingKey = nameof(WorkingKey);
        private const string ExecutingKey = nameof(ExecutingKey);
        private const string PowerKey = nameof(PowerKey);

        /// <summary>
        /// Возвращает "Режим работы"
        /// </summary>
        /// <param name="options"></param>     
        /// <returns></returns>
        public static T GetWorkingMode<T>(this TestOptions options)
        {
            return options.GetProperty<T>(WorkingKey);
        }

        /// <summary>
        /// Устанавливает "Режим работы"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="setting"></param>
        public static void SetWorkingMode<T>(this TestOptions options, T setting)
        {
            options.SetProperty<T>(setting, WorkingKey);
        }


        /// <summary>
        /// Возвращает "Режим проверки"
        /// </summary>
        /// <param name="options"></param>      
        /// <returns></returns>
        public static T GetExecutingMode<T>(this TestOptions options)
        {
            return options.GetProperty<T>(ExecutingKey);
        }

        /// <summary>
        /// Устанавливает "Режим проверки"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="setting"></param>
        public static void SetExecutingMode<T>(this TestOptions options, T setting)
        {
            options.SetProperty<T>(setting, ExecutingKey);
        }


        /// <summary>
        /// Возвращает "Режим источника питания"
        /// </summary>
        /// <param name="options"></param>      
        /// <returns></returns>
        public static T GetPowerMode<T>(this TestOptions options)
        {
            return options.GetProperty<T>(PowerKey);
        }

        /// <summary>
        /// Устанавливает "Режим источника питания"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="setting"></param>
        public static void SetPowerMode<T>(this TestOptions options, T setting)
        {
            options.SetProperty<T>(setting, PowerKey);
        }
    }
}
