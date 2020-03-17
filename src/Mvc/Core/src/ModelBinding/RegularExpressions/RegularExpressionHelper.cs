using System.Text.RegularExpressions;

namespace NLib.AtpNetCore.Mvc.ModelBinding.RegularExpressions
{
    public static class RegularExpressionHelper
    {
        /*
        * Начало основной группы (может повторяться один и более раз)
        * "(?:" +
        * 
        * Имя свойства.
        * "(?<NAME>[_a-zA-Z][\\w]*)" +
        * 
        * Индекс числовой!. (После него не могут идти числа!)
        * "(?:" + "\\[(?<INDEX>\\d+)\\]" + "(?![\\w])" + ")?" +
        * 
        * Символ '.'
        * "\\.?" +
        * 
        * Конец основной группы
        * ")+" +
        * 
        * Строка не может оканчиваться на символ '.'
        * "(?<![.])"
        * 
        */
        public static Regex ProperyRegex = new Regex(
            "(?:(?<NAME>[_a-zA-Z][\\w]*)(?:\\[(?<INDEX>\\d+)\\](?![\\w]))?\\.?)+(?<![.])",
            RegexOptions.Compiled);

        //TODO: Юнит тест!
        public static Regex TextRegex = new Regex(
            "<(?<ORDER>@?)(?:(?<NAME>[_a-zA-Z][\\w]*)(?:\\[(?<INDEX>\\d+)\\](?![\\w]))?\\.?)+(?<![.])>"
           , RegexOptions.Compiled);

    }
}
