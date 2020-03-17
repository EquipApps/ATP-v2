using NLib.AtpNetCore.Mvc.ModelBinding.RegularExpressions;
using System;
using System.Text;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Text
{
    public static class TextModelParser
    {
        //TODO: Доделать юнит тесты
        public static TextModel ParseText(string text)
        {
            if (text == null)
            {
                throw new ArgumentException(nameof(text));
            }

            var matches = RegularExpressionHelper.TextRegex.Matches(text);

            if (matches.Count == 0)
            {
                return new TextModel()
                {
                    Format = text,
                    Inserts = null
                };
            }

            var formatBuilder = new StringBuilder(text);
            var inserts = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                var math = matches[i];

                //if (math.Success)
                //    continue;


                inserts[i] = math.Groups["NAME"].Value;

                formatBuilder.Replace(
                    math.Groups[0].Value, $"{{{i}}}");

            }

            return new TextModel()
            {
                Format = formatBuilder.ToString(),
                Inserts = inserts
            };
        }
    }
}
