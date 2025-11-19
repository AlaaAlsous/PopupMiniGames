using PopupMiniGames.GameAssets.FlappyKalleAssets;
using PopupMiniGames.GameAssets.PolarEngine;

namespace MiniGames.MiniGames
{
    public class FlappyKalle : IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private string assetsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\GameAssets\FlappyKalleAssets"
        );

        Game game = new Game();
        Form form = new Form();

        private int mistakes = 0;
        private int maxMistakes = 10;
        private int score = 0;
        private int maxScore = 10;
        public void StartGame(Difficulty difficulty)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.Width = 1000;
            form.Height = 800;

            MessageBox.Show("Flappy kalle is starting! Be ready to jump with space!", "Get Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);

            game.Renderer.Width = form.Width;
            game.Renderer.Height = form.Height;
            game.Renderer.BackgroundColor = Color.LightSkyBlue;
            form.Controls.Add(game.Renderer);

            PipeSpawner pipeSpawner = new PipeSpawner(Point.Empty, game, this);
            game.AddGameObject(pipeSpawner);
            FlappyController player = new FlappyController(new Point(100, 200), game, Path.Combine(assetsPath, "KalleAnkaFace.png"), this);
            game.AddGameObject(player);
            game.Start();

            form.KeyDown += game.OnKeyDown;
            SetDifficulty(difficulty, pipeSpawner);
            form.ShowDialog();

        }
        public void ChangeScore(int addedScore, int addedMistakes)
        {
            score = Math.Min(score+addedScore, maxScore);
            mistakes = Math.Min(mistakes+addedMistakes, maxMistakes);

            if (mistakes >= maxMistakes)
            { 
                EndGame(false);
            }
            if (score >= maxScore)
            { 
                EndGame(true);
            }

        }
        private void SetDifficulty(Difficulty difficulty, PipeSpawner pipeSpawner)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    pipeSpawner.TimeBetweenSpawns = 6;
                    pipeSpawner.Speed = 120;
                    pipeSpawner.MiddleSpace = 320;
                    break;
                case Difficulty.Medium:
                    pipeSpawner.TimeBetweenSpawns = 2.6f;
                    pipeSpawner.Speed = 200;
                    pipeSpawner.MiddleSpace = 260;
                    break;
                case Difficulty.Hard:
                    pipeSpawner.TimeBetweenSpawns = 1.8f;
                    pipeSpawner.Speed = 360;
                    pipeSpawner.MiddleSpace = 200;
                    break;
            }
        }

        private void EndGame(bool won)
        {
            game.Stop();
            form.Close();
            string resultMessage = won
                ? $"You won with a score of {score}, with {mistakes} mistakes.!"
                : $"Game Over! \n You got a score of {score} with {mistakes} mistakes.";
            MessageBox.Show(resultMessage, "Game Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Cleanup();
            GameEnded?.Invoke(this, new GameResult
            {
                Won = won,
                Points = score,
                Mistakes = mistakes
            });

        }
        public void Cleanup()
        {
            form.KeyDown -= game.OnKeyDown;
            form.Dispose();
            game.Dispose();
        }

    }
}
