using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calcualtor
{
    public partial class Form1 : Form
    {
        private enum NumberMode { Big, Roman, Complex };
        private NumberMode numberMode;
        private BigNumber _resultBig;
        private RomanNumber _resultRoman;
        private ComplexNumber _resultComplex;
        private bool _opratorInProgress = false;
        private string _operationToFinish = "";
        private bool _newComplexInConstruction = false;
        private bool _cxImgInProgress = false;
        public Form1()
        {
            numberMode = NumberMode.Big;
            InitializeComponent();
            RenderScreen();
            button16.Focus();
        }

        private void number_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled == false)
                return;

            if (_opratorInProgress)
            {
                if (numberMode == NumberMode.Complex)
                    screen.Text = "(0+0i)";
                else
                    screen.Text = "";

                _opratorInProgress = false;
            }

            this.screen.Text = RenderScrenNumber(((Button)sender).Text, false, false, false);
        }
        private void operation_Click(object sender, EventArgs e)
        {
            if(numberMode == NumberMode.Complex)
            {
                if ((((Button)sender).Text == "*" || ((Button)sender).Text == "/") && !_newComplexInConstruction)
                    return; // u konstrukciji kompleksnog broja ne moze da postoji puta i podeljeno

                if (!_newComplexInConstruction)
                {
                    if(((Button)sender).Text == "-") // menjamo inicijani plus u -
                    {
                        screen.Text = screen.Text.Replace('+', '-');
                    }

                    _newComplexInConstruction = true;
                    _cxImgInProgress = true;
                    return;
                }
                else
                {
                    _newComplexInConstruction = false;
                    _cxImgInProgress = false;
                }

            }
            _opratorInProgress = true;
            if (numberMode == NumberMode.Big)
                screen.Text = this.BigNumberCalculation();
            else if (numberMode == NumberMode.Roman)
            {
                if (!RomanNumber.CheckIfRoman(screen.Text))
                {
                    MessageBox.Show("Nije ispravan rimski broj");
                    return;
                }
                screen.Text = this.RomanNumberCalculation();
            }
            else
                screen.Text = this.ComplexNumberCalculation();

            if (((Button)sender).Text == "+")
            {
                _operationToFinish = "+";
            }
            if (((Button)sender).Text == "-")
            {
                _operationToFinish = "-";
            }
            if (((Button)sender).Text == "*")
            {
                _operationToFinish = "*";
            }
            if (((Button)sender).Text == "/")
            {
                _operationToFinish = "/";
            }
            if (((Button)sender).Text == "=")
            {
                _operationToFinish = "";
                _cxImgInProgress = false;
            }
        }
        private void changeSign_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled == false)
                return;

            if (numberMode != NumberMode.Complex)
            {
                if (this.screen.Text.Length == 1 && this.screen.Text[0] == '0')
                    return;
            }
            else
            {
                // IndexOf("+", 1) Krecemo od indexa 1 sa pretragom, zato sto zelimo da preskocimo realni deo ako ima predznak npr -2+3i. - treba preskociti
                var _signIndex = this.screen.Text.IndexOf("+", 1);
                if (_signIndex <= 0)
                {
                    _signIndex = this.screen.Text.IndexOf("-", 1);
                }

                var _imgPart = this.screen.Text.Substring(_signIndex + 1, this.screen.Text.Length - _signIndex - 3);
                var _relPart = this.screen.Text.Substring(1, _signIndex - 1);
                var _validatePart = "";
                if (_cxImgInProgress == false)
                {
                    _validatePart = _relPart;
                }
                else
                {
                    _validatePart = _imgPart;
                }
                if (_validatePart.Length == 1 && _validatePart[0] == '0')
                    return;
            }

            this.screen.Text = this.RenderScrenNumber("", false, true, false);

            button16.Select();
            button16.Focus();
        }
        private void decimalPoint_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled == false)
                return;

            this.screen.Text = RenderScrenNumber(".", true, false, false);
        }
        private void backspace_Click(object sender, EventArgs e)
        {
            this.screen.Text = RenderScrenNumber("", false, false, true);
            button16.Select();
            button16.Focus();
        }
        private void delete_Click(object sender, EventArgs e)
        {
            this.RenderScreen();
        }
        private void keyBoard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.NumPad9)
            {
                var _key = e.KeyCode.ToString();
                var _buttons = this.Controls.Find("button" + _key[_key.Length - 1].ToString(), true);
                number_Click(_buttons[0], e);
            }
            else if(e.KeyCode == Keys.Decimal)
            {
                decimalPoint_Click(button15, e);
            }
            else if(e.KeyCode == Keys.Back)
            {
                backspace_Click(button17, e);
            }
            else if(e.KeyCode == Keys.Delete)
            {
                delete_Click(button18, e);
            }
            else if(e.KeyCode == Keys.Add)
            {
                operation_Click(button12, e);
            }
            else if (e.KeyCode == Keys.Subtract)
            {
                operation_Click(button11, e);
            }
            else if (e.KeyCode == Keys.Divide)
            {
                operation_Click(button13, e);
            }
            else if (e.KeyCode == Keys.Multiply)
            {
                operation_Click(button10, e);
            }
        }
        private void Enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                operation_Click(button16, e);
        }
        private void modeChange_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Big number")
                this.numberMode = NumberMode.Big;
            else if (((Button)sender).Text == "Roman number")
                this.numberMode = NumberMode.Roman;
            else
                this.numberMode = NumberMode.Complex;
            
            this.RenderScreen();
        }

        private string BigNumberCalculation()
        {
            var _newEnteredNumnber = new BigNumber(screen.Text);

            if (_operationToFinish == "")
            {
                if (_resultBig is null) // ako nismo jos ni jednom ukucali broj, pa predhodni razultat ne postoji
                {
                    _resultBig = _newEnteredNumnber;
                }
            }

            if (_operationToFinish == "+")
            {
                var _add = _resultBig + _newEnteredNumnber;
                _resultBig = _add;
            }

            if (_operationToFinish == "-")
            {
                var _substract = _resultBig - _newEnteredNumnber;
                _resultBig = _substract;
            }

            if (_operationToFinish == "*")
            {
                var _multiply = _resultBig * _newEnteredNumnber;
                _resultBig = _multiply;
            }

            if (_operationToFinish == "/")
            {
                var _divide = _resultBig / _newEnteredNumnber;
                _resultBig = _divide;
            }

            return _resultBig.ToString();
        }
        private string RomanNumberCalculation()
        {
            var _newEnteredNumnber = new RomanNumber(screen.Text);
            if (_operationToFinish == "")
            {
                if (_resultRoman is null) // ako nismo jos ni jednom ukucali broj, pa predhodni razultat ne postoji
                {
                    _resultRoman = _newEnteredNumnber;
                }
            }

            if (_operationToFinish == "+")
            {
                var _add = _resultRoman + _newEnteredNumnber;
                _resultRoman = _add;
            }

            if (_operationToFinish == "-")
            {
                var _substract = _resultRoman - _newEnteredNumnber;
                _resultRoman = _substract;
            }

            if (_operationToFinish == "*")
            {
                var _multiply = _resultRoman * _newEnteredNumnber;
                _resultRoman = _multiply;
            }

            if (_operationToFinish == "/")
            {
                var _divide = _resultRoman / _newEnteredNumnber;
                _resultRoman = _divide;
            }

            return _resultRoman.ToString();
        }
        private string ComplexNumberCalculation()
        {
            var _newEnteredNumnber = new ComplexNumber(screen.Text);
            if (_operationToFinish == "")
            {
                if (_resultComplex is null) // ako nismo jos ni jednom ukucali broj, pa predhodni razultat ne postoji
                {
                    _resultComplex = _newEnteredNumnber;
                }
            }

            if (_operationToFinish == "+")
            {
                var _add = _resultComplex + _newEnteredNumnber;
                _resultComplex = _add;
            }

            if (_operationToFinish == "-")
            {
                var _substract = _resultComplex - _newEnteredNumnber;
                _resultComplex = _substract;
            }

            if (_operationToFinish == "*")
            {
                var _multiply = _resultComplex * _newEnteredNumnber;
                _resultComplex = _multiply;
            }

            if (_operationToFinish == "/")
            {
                var _divide = _resultComplex / _newEnteredNumnber;
                _resultComplex = _divide;
            }
            return _resultComplex.ToString();
        }

        private void RenderScreen()
        {
            if (numberMode == NumberMode.Complex)
                screen.Text = "(0+0i)";
            else
                screen.Text = "0";
            _operationToFinish = "";
            _resultBig = null;
            _resultRoman = null;
            _resultComplex = null;
            _cxImgInProgress = false;
            _newComplexInConstruction = false;

            if(numberMode == NumberMode.Big)
            {
                bigNumberBtn.FlatStyle = FlatStyle.Flat;
                romanNumberBtn.FlatStyle = FlatStyle.Standard;
                complexNumberBtn.FlatStyle = FlatStyle.Standard;
            }
            else if (numberMode == NumberMode.Roman)
            {
                bigNumberBtn.FlatStyle = FlatStyle.Standard;
                romanNumberBtn.FlatStyle = FlatStyle.Flat;
                complexNumberBtn.FlatStyle = FlatStyle.Standard;

            }
            else
            {
                bigNumberBtn.FlatStyle = FlatStyle.Standard;
                romanNumberBtn.FlatStyle = FlatStyle.Standard;
                complexNumberBtn.FlatStyle = FlatStyle.Flat;

            }
            if (numberMode == NumberMode.Big || numberMode == NumberMode.Complex)
            {
                button0.Enabled = true;
                button0.BackColor = Color.White;
                button1.Text = "1";
                button1.BackColor = Color.White;
                button2.Text = "2";
                button2.BackColor = Color.White;
                button3.Text = "3";
                button3.BackColor = Color.White;
                button4.Text = "4";
                button4.BackColor = Color.White;
                button5.Text = "5";
                button5.BackColor = Color.White;
                button6.Text = "6";
                button6.BackColor = Color.White;
                button7.Text = "7";
                button7.BackColor = Color.White;
                button8.Enabled = true;
                button8.BackColor = Color.White;
                button9.Enabled = true;
                button9.BackColor = Color.White;
                button14.Enabled = true;
                button10.BackColor = Color.White;
                button15.Enabled = true;
                button11.BackColor = Color.White;
            }
            else
            {
                button0.Enabled = false;
                button1.Text = "I";
                button1.BackColor = Color.AntiqueWhite;
                button2.Text = "V";
                button2.BackColor = Color.AntiqueWhite;
                button3.Text = "X";
                button3.BackColor = Color.AntiqueWhite;
                button4.Text = "L";
                button4.BackColor = Color.AntiqueWhite;
                button5.Text = "C";
                button5.BackColor = Color.AntiqueWhite;
                button6.Text = "D";
                button6.BackColor = Color.AntiqueWhite;
                button7.Text = "M";
                button7.BackColor = Color.AntiqueWhite;
                button8.Enabled = false;
                button9.Enabled = false;
                button14.Enabled = false;
                button15.Enabled = false;
            }

            button16.Select();
            button16.Focus();
        }
        private string RenderScrenNumber(string digit, bool pointClick, bool changeSign, bool removeDigit)
        {
            var _screenText = this.screen.Text; // uzimamo sta je trenutno na ekranu

            if(numberMode != NumberMode.Complex)
            {
                _screenText = RenderNumber(_screenText, digit, pointClick, changeSign, removeDigit);
            }
            else 
            {
                var _signIndex = _screenText.IndexOf("+", 2);
                if (_signIndex <= 0)
                {
                    _signIndex = _screenText.IndexOf("-", 2);
                }
                var _sign = _screenText.Substring(_signIndex, 1);
                var _imgPart = _screenText.Substring(_signIndex, _screenText.Length - _signIndex - 2);
                var _relPart = _screenText.Substring(1, _signIndex - 1);
                if (_cxImgInProgress == false)
                {
                    _relPart = RenderNumber(_relPart, digit, pointClick, changeSign, removeDigit);
                    _screenText = "(" + _relPart + _imgPart + "i)";

                }
                else
                {
                    if (!changeSign)//skini znak
                    {
                        _imgPart = _imgPart.Substring(1);
                        _imgPart = RenderNumber(_imgPart, digit, pointClick, changeSign, removeDigit);
                        _screenText = "(" + _relPart + _sign + _imgPart + "i)";
                    }
                    else
                    { 
                        if(_sign == "+")
                            _imgPart = _imgPart.Substring(1);
                        _imgPart = RenderNumber(_imgPart, digit, pointClick, changeSign, removeDigit);
                        if (!_imgPart.Contains("-"))
                        { _imgPart = _imgPart.Insert(0, "+"); }
                        _screenText = "(" + _relPart + _imgPart + "i)";
                    }
                }
            }
            return _screenText;
        }

        // numberPart je realan broj koji dalje menjamo
        // digit je cifra koju dodajemo na kraj, ili ako dodjemo zarez onda se digit ne uzima u obzir
        private string RenderNumber(string numberPart, string digit, bool pointClick, bool signChange, bool removeDigit) 
        {
            var _numberPart = numberPart;
            
            if (pointClick)
            {
                if (!_numberPart.Contains('.'))
                    _numberPart += ".";
            }
            else if(signChange)
            {
                if (_numberPart[0] == '-')
                {
                    _numberPart = _numberPart.Substring(1);
                }
                else
                {
                    _numberPart = "-" + _numberPart;
                }
            }
            else if(removeDigit)
            {
                if (_numberPart.Length == 1)
                    _numberPart = "0";
                _numberPart = _numberPart.Substring(0, _numberPart.Length - 1);
                if (_numberPart.Length == 0)
                    _numberPart = "0";
            }
            else
            {
                if (digit == "0" && _numberPart.Length == 1 && _numberPart[0] == '0')
                    _numberPart = "0";
                else if (_numberPart.Length == 1 && _numberPart[0] == '0')
                    _numberPart = digit;
                else
                    _numberPart += digit;
            }
            return _numberPart;
        }
    }
}
