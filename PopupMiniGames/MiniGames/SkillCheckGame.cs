using MiniGames;
using MiniGames.MiniGames;
using PopupMiniGames.GameAssets.PolarEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGames.MiniGames
{
    internal class SkillCheckGame : Form, IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private string assetsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\GameAssets\SkillCheckAssets"
        );

        private Game game = new Game();

        private int mistakes = 0;
        private int maxMistakes = 4;
        private int score = 0;
        private int maxScore = 10;
        private GameData gameData;

        public SkillCheckGame(GameData gameData)
        {
            this.gameData = gameData;
        }
        public void StartGame(Difficulty difficulty)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = 500;
            this.Height = 500;
            this.CenterToScreen();

            MessageBox.Show("Skillcheck game is starting! Be ready to hit the skillchecks with space!", "Get Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);

            game.Renderer.Width = this.Width;
            game.Renderer.Height = this.Height;
            game.Renderer.BackgroundColor = Color.Black;
            this.Controls.Add(game.Renderer);
            SetDifficulty(difficulty);

           
            game.Start();
            this.KeyDown += game.OnKeyDown;
            this.ShowDialog();
        }
        public void ChangeScore(int addedScore, int addedMistakes)
        {
            score = Math.Min(score + addedScore, maxScore);
            mistakes = Math.Min(mistakes + addedMistakes, maxMistakes);

            if (mistakes >= maxMistakes)
            {
                EndGame(false);
            }
            if (score >= maxScore)
            {
                EndGame(true);
            }

        }
        private void SetDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:

                    break;
                case Difficulty.Medium:

                    break;
                case Difficulty.Hard:

                    break;
            }
        }

        private void EndGame(bool won)
        {
            game.Stop();
            this.Close();
            string resultMessage = won
                ? $"You won!"
                : $"You lost!";
            MessageBox.Show(resultMessage, "Game Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GameEnded?.Invoke(this, new GameResult
            {
                Won = won,
                Points = won ? 10 : 0,
                Mistakes = won ? 0 : 5,
                GameName = "Skillcheck Game"
            });

        }
        public void Cleanup()
        {
            this.KeyDown -= game.OnKeyDown;
            game.Dispose();
            this.Dispose();
        }

    }

}
