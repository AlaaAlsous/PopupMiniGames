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

        private Game game = new Game();
        private Form form = new Form();

        private int mistakes = 0;
        private int maxMistakes = 4;
        private int score = 0;
        private int maxScore = 10; // 2 score per cleared pipe
        private GameData gameData;

        public FlappyKalle(GameData data)
        {
            gameData = data;
        }
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
            addedMistakes = addedMistakes * mistakeMultiplier;
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
        private void SetDifficulty(Difficulty difficulty, PipeSpawner pipeSpawner)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    maxMistakes = 4;
                    maxScore = 10;
                    pipeSpawner.TimeBetweenSpawns = 6;
                    pipeSpawner.Speed = 120;
                    pipeSpawner.MiddleSpace = 320;
                    mistakeMultiplier = 1;
                    break;
                case Difficulty.Medium:
                    maxMistakes = 3;
                    maxScore = 14;
                    pipeSpawner.TimeBetweenSpawns = 2.6f;
                    pipeSpawner.Speed = 200;
                    pipeSpawner.MiddleSpace = 260;
                    mistakeMultiplier = 1;
                    break;
                case Difficulty.Hard:
                    maxMistakes = 3;
                    maxScore = 18;
                    pipeSpawner.TimeBetweenSpawns = 1.8f;
                    pipeSpawner.Speed = 360;
                    pipeSpawner.MiddleSpace = 200;
                    mistakeMultiplier = 2;
                    break;
            }
            maxScore = 10 * scoreRatio;
        }

        private void EndGame(bool won)
        {
            game.Stop();
            form.Close();
            score = score / scoreRatio;
            string resultMessage = won
                ? $"You won!"
                : $"You lost!";
            MessageBox.Show(resultMessage, "Game Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Cleanup();
            GameEnded?.Invoke(this, new GameResult
            {
                Won = won,
                Points = won ? 10 : 0,
                Mistakes = won ? 0 : 5,
                GameName = "Flappy Kalle"
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
