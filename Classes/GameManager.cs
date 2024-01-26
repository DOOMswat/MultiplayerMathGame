using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame.Classes
{
    public enum Difficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }
    internal class GameManager : QuestionGenerator
    {
        private int _trials;
        private Difficulty _difficulty;
        private int _guesses;
        private int _maxGuesses;

        public int Trials
        {
            get { return _trials; }
        }

        public Difficulty Difficulty
        {
            get { return _difficulty; }
            set 
            { 
                _difficulty = value;
                SetDifficulty(value);
            }
        }

        public int Guesses
        {
            get { return _guesses; }
        }

        public int MaxGuesses
        {
            get { return _maxGuesses; }
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

        private void SetDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    _trials = 5;
                    _maxGuesses = 5;
                    break;
                case Difficulty.Medium:
                    _trials = 3;
                    _maxGuesses = 10;
                    break;
                case Difficulty.Hard:
                    _trials = 1;
                    _maxGuesses = 10;
                    break;
            }
        }

        public GameManager(Difficulty difficulty, Operator op)
        {
            _guesses = 0;
            this.Operator = op;
            SetDifficulty(difficulty);
        }
    }
}
