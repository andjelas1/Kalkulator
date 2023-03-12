using System;
using System.Collections.Generic;
using System.Text;

namespace Calcualtor
{
    public class RomanNumber
    {
        private static Dictionary<char, int> RomanNumberDictionary = new Dictionary<char, int>()
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
         };

        private static Dictionary<int, string> NumberRomanDictionary = new Dictionary<int, string>()
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 90, "XC" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" },
        };

        private string _romanNumber = "";
        private BigNumber _bigNumber;

        public RomanNumber(string _number)
        {
            _romanNumber = _number;
            _bigNumber = new BigNumber(ConvertRomanNumberToArabic(_romanNumber));
        }

        public override string ToString()
        {
            return _romanNumber;
        }
        private string ConvertRomanNumberToArabic(string _roman)
        {
            int number = 0;
            for (int i = 0; i < _roman.Length; i++)
            {
                if (i + 1 < _roman.Length && RomanNumberDictionary[_roman[i]] < RomanNumberDictionary[_roman[i + 1]])
                {
                    number -= RomanNumberDictionary[_roman[i]];
                }
                else
                {
                    number += RomanNumberDictionary[_roman[i]];
                }
            }
            return number.ToString();
        }
        private static string ConvertArabicToRoman(int _number)
        {
            var roman = new StringBuilder();

            foreach (var item in NumberRomanDictionary)
            {
                while (_number >= item.Key)
                {
                    roman.Append(item.Value);
                    _number -= item.Key;
                }
            }

            return roman.ToString();
        }

        public static RomanNumber operator +(RomanNumber number1, RomanNumber number2)
        {
            var _result = number1._bigNumber + number2._bigNumber;
            var _convertedBackToRomanNumber = ConvertArabicToRoman(Int32.Parse(_result.ToString()));

            return new RomanNumber(_convertedBackToRomanNumber);
        }
        public static RomanNumber operator -(RomanNumber number1, RomanNumber number2)
        {
            var _result = number1._bigNumber - number2._bigNumber;
            var _convertedBackToRomanNumber = ConvertArabicToRoman(Int32.Parse(_result.ToString()));

            return new RomanNumber(_convertedBackToRomanNumber);
        }
        public static RomanNumber operator *(RomanNumber number1, RomanNumber number2)
        {
            var _result = number1._bigNumber * number2._bigNumber;
            var _convertedBackToRomanNumber = ConvertArabicToRoman(Int32.Parse(_result.ToString()));

            return new RomanNumber(_convertedBackToRomanNumber);
        }
        public static RomanNumber operator /(RomanNumber number1, RomanNumber number2)
        {
            var _result = number1._bigNumber / number2._bigNumber;
            var _rounded = _result.ConvertToInt();
            var _convertedBackToRomanNumber = ConvertArabicToRoman(Int32.Parse(_rounded.ToString()));

            return new RomanNumber(_convertedBackToRomanNumber);
        }
    }
}
