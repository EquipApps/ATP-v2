namespace B.EK.Configure
{
    public static class Settings
    {
        public static string WorkingMode_NS { get; } = "НСТ";
        public static string WorkingMode_ZI { get; } = "ЗИ";
        public static string WorkingMode_PC { get; } = "ПСИ";


        public static string PowerMode_MIN { get; } = "Минимум";
        public static string PowerMode_NOM { get; } = "Номинал";
        public static string PowerMode_MAX { get; } = "Максимум";


        public static string ExecutingMode_Main { get; } = "Основной";
        public static string ExecutingMode_Operate { get; } = "Наработка";
        public static string ExecutingMode_Power { get; } = "10 Циклов подачи питания";

    }
}
