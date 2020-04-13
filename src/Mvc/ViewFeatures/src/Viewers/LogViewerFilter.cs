namespace EquipApps.Mvc.Viewers
{
    public class LogViewerFilter
    {
        public static bool TRUE(LogEntry logEntry)
        {
            return true;
        }
        public static bool FALSE(LogEntry logEntry)
        {
            return false;
        }
    }
}
