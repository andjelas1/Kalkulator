using System;

namespace Calcualtor
{
    public class BigNumber
    {
        private int _decimalIndex = -1;
        private int _leftDigitsNumber = 0;
        private int _rightDigitsNumber = 0;
        private int[] _digits;
        private bool _isNegative;
        private bool _isDecimal;
        public BigNumber(string _number)
        {
            _isNegative = false;
            if (_number.Length > 1 && _number[0] == '-')
            {
                _isNegative = true;
                _number = _number.Substring(1); // prepravi ulazni broj da nema znak -
            }
            if (_number.Contains('.'))
            {
                _isDecimal = true;
                _decimalIndex = _number.IndexOf(".");
            }
            if(_isDecimal) 
                _digits = new int[_number.Length-1]; // niz ce biti za jedan manje duzine od ulaznog jer necemo zarez da upisujemo
            else
                _digits = new int[_number.Length];

            for (int i = 0; i < _number.Length; i++)
            {
                if (i == _decimalIndex)
                    continue;
                if(i > _decimalIndex && _isDecimal)
                {
                    _digits[i - 1] = int.Parse(_number[i].ToString());
                    _rightDigitsNumber++;
                }
                else
                {
                    _digits[i] = int.Parse(_number[i].ToString());
                    _leftDigitsNumber++;
                }
            }
        }
       
        public BigNumber ConvertToInt()
        {
            string _newNumber = "";
            if (_isNegative)
                _newNumber += "+";
            for (int i = 0; i < _leftDigitsNumber; i++)
            {
                _newNumber += _digits[i].ToString();
            }

            return new BigNumber(_newNumber);
        }

        public int Lenght()
        {
            return _digits.Length;
        }
        public bool IsNegative
        {
            get { return _isNegative; }
        }
        public void Negate()
        {
            _isNegative = !_isNegative;
        }
        public BigNumber Abs()
        {
            var _absNumber = new BigNumber(this.ToString());
            if (_absNumber._isNegative)
                _absNumber.Negate();
            return _absNumber;
        }
        public override string ToString()
        {
            string result = "";
            if (_isNegative)
            {
                result = "-" + result;
            }
            for (int i = 0; i < _digits.Length; i++)
            {
                result += _digits[i].ToString();
            }
            var _pomIndex = _decimalIndex;
            if (_isDecimal)
            {
                if (_isNegative)
                    _pomIndex += 1;
                result = result.Substring(0, _pomIndex) + "." + result.Substring(_pomIndex);
            }

            return result;
        }

        public static bool operator ==(BigNumber number1, BigNumber number2)
        {
            var _equal = true;
            if(number1.Lenght() != number2.Lenght() || number1._isNegative != number2._isNegative)
                return false;
            for(int i = 0; i < number1.Lenght(); i++)
                if(number1._digits[i] != number2._digits[i])
                {
                    _equal = false;
                    break;
                }
            return _equal;
        }
        public static bool operator !=(BigNumber number1, BigNumber number2)
        {
            var _notEqual = false;
            if (number1.Lenght() != number2.Lenght() || number1._isNegative != number2._isNegative)
                return true;
            for (int i = 0; i < number1.Lenght(); i++)
                if (number1._digits[i] != number2._digits[i])
                {
                    _notEqual = true;
                    break;
                }

            return _notEqual;
        }
        public static bool operator >(BigNumber number1, BigNumber number2)
        {
            if (!number1._isNegative && number2._isNegative)
                return true;
            else if (number1._isNegative && !number2._isNegative)
                return false;
            else if (!number1._isNegative && !number2._isNegative)
            {
                if (number1._leftDigitsNumber > number2._leftDigitsNumber)
                    return true;
                else if(number1._leftDigitsNumber < number2._leftDigitsNumber)
                    return false;
                else
                {
                    var _isBigger = false;
                    for (int i = 0; i < number1.Lenght(); i++)
                    {
                        if (number1._digits[i] > number2._digits[i])
                        {
                            _isBigger = true;
                            break;
                        }
                        else if (number1._digits[i] < number2._digits[i])
                        {
                            break;
                        }
                        else
                            continue;
                    }
                    return _isBigger;
                }
            }
            else
            {
                if (number1._leftDigitsNumber < number2._leftDigitsNumber)
                    return true;
                else if (number1._leftDigitsNumber > number2._leftDigitsNumber)
                    return false;
                else
                {
                    var _isBigger = false;
                    for (int i = 0; i < number1.Lenght(); i++)
                    {
                        if (number1._digits[i] < number2._digits[i])
                        {
                            _isBigger = true;
                            break;
                        }
                        else if (number1._digits[i] > number2._digits[i])
                        {
                            break;
                        }
                        else
                            continue;
                    }
                    return _isBigger;
                }
            }
        }
        public static bool operator <(BigNumber number1, BigNumber number2)
        {
            return number2 > number1;
        }
        public static bool operator >=(BigNumber number1, BigNumber number2)
        {
            if (number1 == number2)
                return true;
            else
                return number1 > number2;
        }
        public static bool operator <=(BigNumber number1, BigNumber number2)
        {
            if (number1 == number2)
                return true;
            else
                return number2 > number1;
        }

        public static BigNumber operator +(BigNumber number1, BigNumber number2)
        {
            string result;
            if (number1._isNegative && !number2._isNegative) // jedan je negativan, pa treba oduzimati
            {
                number1.Negate(); // posto + operaciju, pretvaramo u minus, da ne bi izgubili znak, number1 pretvaramo u pozitivan
                return number2 - number1;
            }
            else if(!number1._isNegative && number2._isNegative)
            {
                number2.Negate(); // posto + operaciju, pretvaramo u minus, da ne bi izgubili znak, number2 pretvaramo u pozitivan
                return number1 - number2;
            }
            else
            { 
                result = AddBigNumbers(number1, number2);
                
                if(number1._isNegative && number2._isNegative) // ako su oba bila negativna onda je to sabiranje
                    result = result.Insert(0, "-");
            }
            return new BigNumber(result);
        }
        private static string AddBigNumbers(BigNumber number1, BigNumber number2)
        {
            string _resulNumber = "";
            int[] _firstNumberDigitsDecorated;
            int[] _secondNumberDigitsDecorated;

            PrepareDigits(number1, number2, out _firstNumberDigitsDecorated, out _secondNumberDigitsDecorated);

            int[] _resultDigits = new int[_firstNumberDigitsDecorated.Length + 1];
            var carry = 0;
            for(int i= _firstNumberDigitsDecorated.Length-1; i>=0; i--)
            {
                var _sum = _firstNumberDigitsDecorated[i] + _secondNumberDigitsDecorated[i] + carry;
                carry = _sum / 10;
                _resultDigits[i+1] = carry > 0 ? _sum - carry * 10 : _sum;
            }
            if(carry > 0)
                _resultDigits[0] = carry;
            var _decimalPosition = 0;
            //_decimalPosition += number1._decimalIndex >= number2._decimalIndex ? number1._decimalIndex + 1 : number2._decimalIndex + 1;

            _decimalPosition += number1._leftDigitsNumber >= number2._leftDigitsNumber ? number1._leftDigitsNumber + 1 : number2._leftDigitsNumber + 1;

            for (int i =0; i < _resultDigits.Length; i++)
            {
                if (i == _decimalPosition && _decimalPosition != 0)
                    _resulNumber = _resulNumber + ".";
                _resulNumber = _resulNumber + _resultDigits[i];
            }

            return RemoveZeros(_resulNumber);
        }

        public static BigNumber operator -(BigNumber number1, BigNumber number2)
        {
            string result;
            if (!number1._isNegative && number2._isNegative) // bice sabiranje, samo ovaj drugi treba namestiti da je pozitivan predznak
            {
                number2.Negate();
                return number1 + number2;
            }
            if (number1._isNegative && !number2._isNegative) // bice sabiranje, samo ovaj drugi treba namestiti da je pozitivan predznak
            {
                number2.Negate();
                return number1 + number2;
            }

            if (number2._isNegative) // ako je drugi negativan a u pitanju je minus npr, -(-a) onda je dovoljno da a bude +
            {
                number2.Negate();
            }

            if (number1.Abs() >= number2.Abs())
            {
                result = SubtractBigNumbers(number1, number2);
                if (number1._isNegative)
                    result = result.Insert(0, "-");
            }
            else
            {
                result = SubtractBigNumbers(number2, number1);
                if (!number2._isNegative && !number1._isNegative) // oba pozitivna, a zamenili mesta pri oduzimanju, onda ce rezultat sigurno biti negativan broj
                    result = result.Insert(0, "-");

            }
            return new BigNumber(result);
        }
        private static string SubtractBigNumbers(BigNumber number1, BigNumber number2)
        {
            string _resulNumber = "";
            int[] _firstNumberDigitsDecorated;
            int[] _secondNumberDigitsDecorated;

            PrepareDigits(number1, number2, out _firstNumberDigitsDecorated, out _secondNumberDigitsDecorated);

            int[] _resultDigits = new int[_firstNumberDigitsDecorated.Length];

            var borrowed = false;
            for (int i = _resultDigits.Length - 1; i >= 0; i--)
            {
                var _upperDigit = _firstNumberDigitsDecorated[i];
                var _lowerDigit = _secondNumberDigitsDecorated[i];

                _upperDigit = borrowed? _upperDigit -1: _upperDigit;
                if (_upperDigit < _lowerDigit) 
                { 
                    _upperDigit += 10;
                    borrowed = true;
                }
                else
                    borrowed = false;

                var _sum = _upperDigit - _lowerDigit;
                _resultDigits[i] = _sum;
            }

            var _decimalPosition = number1._decimalIndex >= number2._decimalIndex ? number1._decimalIndex : number2._decimalIndex;
            for (int i = 0; i < _resultDigits.Length; i++)
            {
                if (i == _decimalPosition && _decimalPosition != 0)
                    _resulNumber = _resulNumber + ".";
                _resulNumber = _resulNumber + _resultDigits[i];
            }

            return RemoveZeros(_resulNumber);
        }

        public static BigNumber operator *(BigNumber number1, BigNumber number2)
        {
            var result = MultiplyBignumbers(number1, number2);
            var _negativeResult = (number1._isNegative & !number2._isNegative) | (!number1._isNegative & number2._isNegative) ? true : false;
            if(_negativeResult)
                result.Negate();
            return result;
        }
        private static BigNumber MultiplyBignumbers(BigNumber number1, BigNumber number2)
        {
            var _rezult = new BigNumber("0");
            int _num2RightDigits = number2._rightDigitsNumber + 1;
            int _num2LeftDigits = -1;

            for (int i = number2.Lenght() - 1; i >= 0; i--)
            {
                var _currentlyRightNum2 = false;
                var _weightPlaceNumber2 = 0;

                if (number2._isDecimal && i >= number2._decimalIndex)
                {
                    _currentlyRightNum2 = true;
                    _weightPlaceNumber2 = --_num2RightDigits;
                }
                else if (number2._isDecimal && i < number2._decimalIndex)
                {
                    _weightPlaceNumber2 = ++_num2LeftDigits;

                }
                if (!number2._isDecimal)
                {
                    _weightPlaceNumber2 = ++_num2LeftDigits;
                }
                int _num1RightDigits = number1._rightDigitsNumber + 1;
                int _num1LeftDigits = -1;

                for (int j = number1.Lenght() - 1; j >= 0; j--)
                {
                    string _tempCalculation = "";
                    var _multiplication = number2._digits[i] * number1._digits[j];

                    var _weightPlaceNumber1 = 0;
                    var _currentlyRightNum1 = false;

                    if (number1._isDecimal && j >= number1._decimalIndex)
                    {
                        _currentlyRightNum1 = true;
                        _weightPlaceNumber1 = --_num1RightDigits;
                    }
                    else if (number1._isDecimal && j < number1._decimalIndex)
                    {
                        _weightPlaceNumber1 = ++_num1LeftDigits;

                    }
                    if (!number1._isDecimal)
                    {
                        _weightPlaceNumber1 = ++_num1LeftDigits;
                    }
                    if (_multiplication == 0)
                        continue;
                    var _weightPlace = 0;

                    if (_currentlyRightNum1 && _currentlyRightNum2)
                    {
                        _weightPlace = _weightPlaceNumber2 + _weightPlaceNumber1;
                    }
                    else if (!_currentlyRightNum1 && _currentlyRightNum2)
                    {
                        _weightPlace = _weightPlaceNumber2 - _weightPlaceNumber1;
                    }
                    else if (_currentlyRightNum1 && !_currentlyRightNum2)
                    {
                        _weightPlace = _weightPlaceNumber1 - _weightPlaceNumber2;
                    }
                    else
                    {
                        _weightPlace = -1*(_weightPlaceNumber2 +_weightPlaceNumber1);
                    }
                    var _insertDecimal = false;
                    if(_multiplication >= 10 && _weightPlace == 1)
                    {
                        _insertDecimal = true;
                        _weightPlace -= 1;
                    }
                    if(_multiplication >= 10 && _weightPlace > 1)
                        _weightPlace -=1;
                    if (_weightPlace > 0)
                    {
                        _tempCalculation = "0.";
                        _weightPlace -= 1;
                        for (int k = 0; k < _weightPlace; k++) { _tempCalculation = _tempCalculation + "0"; }
                        _tempCalculation = _tempCalculation + _multiplication.ToString();
                        _rezult = _rezult + new BigNumber(_tempCalculation);
                    }
                    else
                    {
                        _weightPlace *= -1;
                        _tempCalculation = _multiplication.ToString();
                        if (_insertDecimal)
                            _tempCalculation = _tempCalculation.Insert(1, ".");
                        for (int l = 0; l < _weightPlace; l++) { _tempCalculation = _tempCalculation + "0"; }
                        _rezult = _rezult + new BigNumber(_tempCalculation);

                    }
                }
            }
            return _rezult;
        }

        public static BigNumber operator /(BigNumber number1, BigNumber number2)
        {
            var result = DivideBignumbers(number1, number2);
            return result;
        }
        private static BigNumber DivideBignumbers(BigNumber number1, BigNumber number2)
        {
            var _result = new BigNumber("0");
            decimal _numberOne = decimal.Parse(number1.ToString());
            decimal _numberTwo = decimal.Parse(number2.ToString());
            var _temp = Math.Round(_numberOne / _numberTwo, 5);
            _result = new BigNumber(_temp.ToString());
            return _result;
        }

        private static string RemoveZeros(string _number)
        {
            _number = _number.TrimStart('0');
            if(_number.Contains('.'))
                _number = _number.TrimEnd('0');
            if(_number == "" || _number == ".")
                _number = "0";
            return _number;
        }
        private static void PrepareDigits(BigNumber number1, BigNumber number2, out int[] firstNumberDigitsDecorated, out int[] secondNumberDigitsDecorated)
        {
            int _numberOfDigitsInResult = 0;
            if (number1._leftDigitsNumber >= number2._leftDigitsNumber)
                _numberOfDigitsInResult += number1._leftDigitsNumber;
            else
                _numberOfDigitsInResult += number2._leftDigitsNumber;

            if (number1._rightDigitsNumber >= number2._rightDigitsNumber)
                _numberOfDigitsInResult += number1._rightDigitsNumber;
            else
                _numberOfDigitsInResult += number2._rightDigitsNumber;

            firstNumberDigitsDecorated = new int[_numberOfDigitsInResult];
            secondNumberDigitsDecorated = new int[_numberOfDigitsInResult];

            var _startIndex = 0;
            var _endIndex = 0;
            if (number1._leftDigitsNumber >= number2._leftDigitsNumber)
                _startIndex = 0;
            else
                _startIndex = number2._leftDigitsNumber - number1._leftDigitsNumber;

            for (int i = 0; i < number1.Lenght(); i++)
            {
                var _digit = number1._digits[i];
                firstNumberDigitsDecorated[_startIndex + i] = number1._digits[i];
            }

            if (number2._leftDigitsNumber >= number1._leftDigitsNumber)
                _startIndex = 0;
            else
                _startIndex = number1._leftDigitsNumber - number2._leftDigitsNumber;

            for (int i = 0; i < number2.Lenght(); i++)
            {
                var _digit = number2._digits[i];
                secondNumberDigitsDecorated[_startIndex + i] = number2._digits[i];
            }
        }
    }
}
