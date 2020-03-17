namespace B.EK.Configure
{
    public static class Settings
    {
        public static string Operation_NS { get; } = "НСТ";
        public static string Operation_ZI { get; } = "ЗИ";
        public static string Operation_PC { get; } = "ПСИ";


        public static string POW_MIN { get; } = "Минимум";
        public static string POW_NOM { get; } = "Номинал";
        public static string POW_MAX { get; } = "Максимум";


        public static string CHECK_MAIN { get; } = "Основной";
        public static string CHECK_OP { get; } = "Наработка";
        public static string CHECK_POW { get; } = "10 Циклов подачи питания";

    }
}
