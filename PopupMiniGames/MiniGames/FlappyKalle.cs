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

            MessageBox.Show("Flappy kalle is starting!", "Get Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);

            game.Renderer.Width = form.Width;
            game.Renderer.Height = form.Height;
            game.Renderer.BackgroundColor = Color.LightSkyBlue;
            form.Controls.Add(game.Renderer);


            
            form.KeyDown += game.OnKeyDown;
            SetDifficulty(difficulty);
            form.ShowDialog();

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


        public void Cleanup()
        {
            form.KeyDown -= game.OnKeyDown;
            form.Dispose();
            game.Dispose();
        }

    }
}
