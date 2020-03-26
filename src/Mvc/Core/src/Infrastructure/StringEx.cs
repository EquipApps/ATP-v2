using System.Text;

namespace EquipApps.Mvc.Internal
{
    public static class StringEx
    {
        /// <summary>
        /// Возвращает число в конце строки
        /// </summary>      
        public static int? ToIntFromEnd(this string target)
        {
            if (target == null)
                return null;

            var dsBuilder = new StringBuilder();

            for (int i = target.Length - 1; i >= 0; i--)
            {
                var dsItem = target[i];

                if (char.IsDigit(dsItem))
                    dsBuilder.Insert(0, dsItem);
                else
                    break;
            }

            if (dsBuilder.Length == 0)
                return null;

            return int.Parse(dsBuilder.ToString());
        }
    }
}
