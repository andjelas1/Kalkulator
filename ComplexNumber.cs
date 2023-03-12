namespace Calcualtor
{
    public class ComplexNumber
    {
        private BigNumber _relPart;
        private BigNumber _imgPart;
        public ComplexNumber(string _number)
        {
            _relPart = new BigNumber("0");
            _imgPart = new BigNumber("0");

            var _temp = "";
            var _imgIsNegative = false;
            for (var i = 1; i < _number.Length-1; i++)
            {
                if(_number[i] == '+' || _number[i] == '-' && i != 1)
                {
                    this._relPart = new BigNumber(_temp);
                    _temp = "";
                    if (_number[i] == '-')
                        _imgIsNegative = true;
                }
                else if(_number[i] == 'i')
                {
                    if (_imgIsNegative)
                        _temp = _temp.Insert(0, "-");
                    _imgPart = new BigNumber(_temp);
                    _temp = "";
                }
                else
                    _temp += _number[i];
            }
            if(_temp != "")
            {
                _relPart = new BigNumber(_temp);
            }
        }

        public ComplexNumber (BigNumber _relPartNumber, BigNumber _imgPartNumber)
        {
            this._relPart = _relPartNumber;
            this._imgPart = _imgPartNumber;
        }
        public override string ToString()
        {
            var _temp = "(";
            
            _temp += _relPart.ToString();

            _temp += _imgPart.IsNegative ? "" : "+";
            _temp += _imgPart.ToString() + "i";
            _temp += ")";
            return _temp;
        }
        public ComplexNumber ConjugatedComplex()
        {
            var _newImgPart = new BigNumber("-1") * this._imgPart;
            var _newRelPart = this._relPart;

            return new ComplexNumber(_newRelPart, _newImgPart);
        }
        public static ComplexNumber operator +(ComplexNumber number1, ComplexNumber number2)
        {
            var _resultRel = number1._relPart + number2._relPart;
            var _resultImg = number1._imgPart + number2._imgPart;
            return new ComplexNumber(_resultRel, _resultImg);
        }
        public static ComplexNumber operator -(ComplexNumber number1, ComplexNumber number2)
        {
            var _resultRel = number1._relPart - number2._relPart;
            var _resultImg = number1._imgPart - number2._imgPart;
            return new ComplexNumber(_resultRel, _resultImg);
        }
        public static ComplexNumber operator *(ComplexNumber number1, ComplexNumber number2)
        {
            var _resultRel = number1._relPart*number2._relPart + number1._imgPart * number2._imgPart * (new BigNumber("-1"));
            var _resultImg = number1._relPart * number2._imgPart + number1._imgPart * number2._relPart;
            return new ComplexNumber(_resultRel, _resultImg);
        }
        public static ComplexNumber operator /(ComplexNumber number1, ComplexNumber number2)
        {
            var _conjugate = number2.ConjugatedComplex();
            var _resultOne = number1 * _conjugate;
            var _divider = number2._relPart * number2._relPart + number2._imgPart * number2._imgPart; //(new BigNumber("-1"))

            var _resultRel = _resultOne._relPart / _divider;
            var _resultImg = _resultOne._imgPart / _divider;

            return new ComplexNumber(_resultRel, _resultImg);
        }
    }
}
