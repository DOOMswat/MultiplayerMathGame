using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame.Classes
{
    enum OperatorM
    {
        Multiply = 0,
        Divide = 1,
        Subtract = 2,
        Add = 3
    }
    internal class QuestionGeneratorM
    {
        private int _firstNummerand;
        private int _secondNummerand;
        private OperatorM _OperatorM;
        private Random _rand = new Random();

        public int FirstNumerand
        {
            get { return _firstNummerand; }
        }
        public int SecondNumerand
        {
            get { return _secondNummerand; }
        }
        public OperatorM OperatorM
        {
            get { return _OperatorM; }
            set { _OperatorM = value; }
        }

        public void RestartQuestion()
        {
            _firstNummerand = 0;
            _secondNummerand = 0;
            _OperatorM = this.OperatorM;
        }

        public void GenerateQuestion()
        {
            _firstNummerand = _rand.Next(20);
            _secondNummerand = _rand.Next(20);
            _OperatorM = this.OperatorM;
        }

        public string DisplayQuestion()
        {
            string output = String.Empty;
            if (_firstNummerand == null ||
                _secondNummerand == null ||
                _OperatorM == null) return output;

            output = $"{_firstNummerand} {ConvertStringToSymbol(_OperatorM)} {_secondNummerand} = ?";

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
                    switch (_OperatorM)
                    {
                        case OperatorM.Add:
                            result = _firstNummerand + _secondNummerand;
                            break;
                        case OperatorM.Subtract:
                            result = _firstNummerand - _secondNummerand;
                            break;
                        case OperatorM.Multiply:
                            result = _firstNummerand * _secondNummerand;
                            break;
                        case OperatorM.Divide:
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



        protected string ConvertStringToSymbol(OperatorM op)
        {
            string symbol = String.Empty;
            switch (op)
            {
                case OperatorM.Add:
                    symbol = "+";
                    break;
                case OperatorM.Subtract:
                    symbol = "-";
                    break;
                case OperatorM.Multiply:
                    symbol = "*";
                    break;
                case OperatorM.Divide:
                    symbol = "/";
                    break;
            }

            return symbol;
        }

    }
}
