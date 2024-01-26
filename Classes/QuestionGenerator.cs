using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame.Classes
{
    enum Operator
    {
        Multiply = 0,
        Divide = 1,
        Subtract = 2,
        Add = 3
    }
    internal class QuestionGenerator
    {
        private int _firstNummerand;
        private int _secondNummerand;
        private Operator _operator;
        private Random _rand = new Random();

        public int FirstNumerand
        {
            get { return _firstNummerand; }
        }
        public int SecondNumerand
        {
            get { return _secondNummerand; }
        }
        public Operator Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        public void RestartQuestion()
        {
            _firstNummerand = 0;
            _secondNummerand = 0;
            _operator = this.Operator;
        }

        public void GenerateQuestion()
        {
            _firstNummerand = _rand.Next(20);
            _secondNummerand = _rand.Next(20);
            _operator = this.Operator;
        }

        public string DisplayQuestion()
        {
            string output = String.Empty;
            if (_firstNummerand == null ||
                _secondNummerand == null ||
                _operator == null) return output;

            output = $"{_firstNummerand} {ConvertStringToSymbol(_operator)} {_secondNummerand} = ?";

            return output;
        }



        public bool CheckAnswer(string userInput)
        {
            bool result = false;
            string value = userInput;
            int numValue;
            bool parsedValue = Int32.TryParse(value, out numValue);
            if (parsedValue)
            {
                result = numValue == CalculateResult();
            }

            return result;
        }

            public int CalculateResult()
            {
                int result = 0;
                try
                {
                    switch (_operator)
                    {
                        case Operator.Add:
                            result = _firstNummerand + _secondNummerand;
                            break;
                        case Operator.Subtract:
                            result = _firstNummerand - _secondNummerand;
                            break;
                        case Operator.Multiply:
                            result = _firstNummerand * _secondNummerand;
                            break;
                        case Operator.Divide:
                            result = _firstNummerand / _secondNummerand;
                            break;
                    }
                }
                catch (DivideByZeroException ex)
                {
                    result = 0;
                }

                return result;
            }



        protected string ConvertStringToSymbol(Operator op)
        {
            string symbol = String.Empty;
            switch (op)
            {
                case Operator.Add:
                    symbol = "+";
                    break;
                case Operator.Subtract:
                    symbol = "-";
                    break;
                case Operator.Multiply:
                    symbol = "*";
                    break;
                case Operator.Divide:
                    symbol = "/";
                    break;
            }

            return symbol;
        }

    }
}
