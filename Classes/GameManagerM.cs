using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame.Classes
{
    
    public enum DifficultyM
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }
    internal class GameManagerM : QuestionGeneratorM
    {

        public static string username = null;
        public static int highestScore = 0;
        
        private int _trials;
        private int _Points;
        private DifficultyM _difficultyM;
        private int _guesses;
        private int _maxGuesses;
        public int Trials
        {
            get { return _trials; }
        }

        public DifficultyM DifficultyM
        {
            get { return _difficultyM; }
            set 
            {
                _difficultyM = value;
                SetDifficulty(value);
            }
        }

        public int Points
        {
            get { return _Points; }
        }
        public int Guesses
        {
            get { return _guesses; }
        }

        public int MaxGuesses
        {
            get { return _maxGuesses; }
        }

        public void AddPoints()
        {
            _Points++;
        }
        public void AddGuess()
        {
            if (_guesses > _maxGuesses) return;
            _guesses++;
        }

        public void RemoveTrial()
        {
            if (_trials < 0) return;
            _trials--;
        }

        private void SetDifficulty(DifficultyM difficultyM)
        {
            switch (difficultyM)
            {
                case DifficultyM.Easy:
                    _trials = 3;
                    _maxGuesses = int.MaxValue;
                    break;
                case DifficultyM.Medium:
                    _trials = 3;
                    _maxGuesses = int.MaxValue;
                    break;
                case DifficultyM.Hard:
                    _trials = 3;
                    _maxGuesses = int.MaxValue;
                    break;
            }
        }

        public GameManagerM(DifficultyM difficultyM, OperatorM op)
        {
            _guesses = 0;
            this.OperatorM = op;
            SetDifficulty(difficultyM);
        }
    }

    
}
