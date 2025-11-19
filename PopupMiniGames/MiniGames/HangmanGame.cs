using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MiniGames;

namespace MiniGames.MiniGames
{
    public class HangmanGame : Form, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private string secretWord = "";
        private HashSet<char> guessedLetters = new HashSet<char>();
        private Label labelWord, labelInfo, currentDifficultyLabel;
        private TextBox inputBox;
        private Button guessButton;
        private int mistakes = 0, maxMistakes = 0;
        private Difficulty currentDifficulty;
        private readonly List<string> easyWords = new List<string>()
        {
            "CAT", "DOG", "CAR", "SUN", "MAP","SEE","CUP",
            "HAT", "PEN", "BED", "BUS", "KEY","BOY","COW"
        };
        private readonly List<string> mediumWords = new List<string>()
        {
            "HOME", "CODE", "CHAT", "CITY", "FISH","DUCK",
            "WALL", "BOOK", "GAME", "LION", "WIND",
        };
        private readonly List<string> hardWords = new List<string>()
        {
            "APPLE", "HOUSE", "BRAIN", "LIGHT", "SOUND","HUMAN",
            "WATER", "STONE", "SMILE", "PLANT", "HEART",
        };
    }
}
