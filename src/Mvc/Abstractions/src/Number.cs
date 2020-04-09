using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static EquipApps.Mvc.Number;

namespace EquipApps.Mvc
{
    /// <summary>
    /// Идентификатор.. 1.1.1.
    /// </summary>
    public class Number : IComparable<Number>, IComparable, IEquatable<Number>
    {
        private string stringView;
        internal readonly TestNumberPart[] testIdParts;

        internal Number(TestNumberPart[] testIdParts)
        {
            if (testIdParts.Length == 0)
                throw new IndexOutOfRangeException();

            this.testIdParts = testIdParts;
        }
        internal Number(IEnumerable<TestNumberPart> testIdParts)
            : this(testIdParts.ToArray())
        {
        }


        #region CompareTo

        public int CompareTo(Number other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            //-- Определям минимальную длину
            var length = Math.Min(testIdParts.Length, other.testIdParts.Length);

            //-- 
            for (int i = 0; i < length; i++)
            {
                var result = testIdParts[i].CompareTo(other.testIdParts[i]);

                if (result != 0)
                    return result;
            }

            //-- Прошли весь массив, все элементы совпали (Проверка по длине)
            return testIdParts.Length.CompareTo(other.testIdParts.Length);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Number);
        }

        #endregion


        public bool Equals(Number other)
        {
            if (other == null)
                return false;

            return CompareTo(other) == 0;
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as Number);
        }

        public override int GetHashCode()
        {
            //return ToString().GetHashCode();

            return base.GetHashCode();
        }




        public override string ToString()
        {
            if (stringView == null)
            {
                var stringBuilder = new StringBuilder();

                for (int i = 0; i < testIdParts.Length; i++)
                {
                    stringBuilder.Append(testIdParts[i].GetString());
                    stringBuilder.Append('.');
                }

                stringView = stringBuilder.ToString();
            }

            return stringView;
        }










        /// <summary>
        /// 
        /// </summary>
        public static implicit operator Number(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            var textIdParts = id.Split('.');
            if (textIdParts.Length == 0)
                throw new InvalidOperationException("Split count 0");

            var testIdParts = new TestNumberPart[textIdParts.Length];

            for (int i = 0; i < textIdParts.Length; i++)
            {
                if (int.TryParse(textIdParts[i], out int digital))
                {
                    testIdParts[i] = new TestNumberPartInt(digital);
                }
                else
                {
                    testIdParts[i] = new TestNumberPartStr(textIdParts[i]);
                }
            }

            return new Number(testIdParts);
        }


        internal abstract class TestNumberPart : IComparable<TestNumberPart>, IComparable
        {
            public abstract bool IsDigital { get; }

            /// <summary>
            /// Если идентификатор не является числовым, то данная функция создаст исключение!
            /// </summary>
            /// <returns></returns>
            public abstract int GetDigital();
            public abstract string GetString();

            public int CompareTo(TestNumberPart other)
            {
                if (IsDigital && other.IsDigital)
                {
                    return GetDigital().CompareTo(other.GetDigital());
                }
                else
                {
                    return GetString().CompareTo(other.GetString());
                }
            }

            public int CompareTo(object obj)
            {
                if (obj is TestNumberPart)
                    return CompareTo((TestNumberPart)obj);

                throw new InvalidOperationException();
            }
        }

        internal class TestNumberPartInt : TestNumberPart
        {
            //--
            private int digital;

            //--
            public TestNumberPartInt(int digital)
            {
                this.digital = digital;
            }

            //--
            public override bool IsDigital => true;

            //--
            public override int GetDigital()
            {
                return digital;
            }

            //--
            public override string GetString()
            {
                return digital.ToString();
            }


        }

        internal class TestNumberPartStr : TestNumberPart
        {
            private string str;

            public TestNumberPartStr(string str)
            {
                this.str = str;
            }

            public override bool IsDigital => false;

            public override int GetDigital()
            {
                throw new InvalidOperationException();
            }

            public override string GetString()
            {
                return str;
            }
        }
    }


    public class TestNumberBuilder
    {
        private readonly List<TestNumberPart> testNumberParts = new List<TestNumberPart>();

        public TestNumberBuilder()
        {

        }

        //TODO: Оптимизировать создание.
        public TestNumberBuilder Append(string number)
        {
            if(!string.IsNullOrWhiteSpace(number))
            {
                Number testNumber = number;
                testNumberParts.AddRange(testNumber.testIdParts);
            }

            return this;
        }

        public TestNumberBuilder Append(Number number)
        {
            if (number != null)
            {
                testNumberParts.AddRange(number.testIdParts);
            }



            return this;
        }

        public Number Build()
        {
            return new Number(testNumberParts);
        }




    }
}
