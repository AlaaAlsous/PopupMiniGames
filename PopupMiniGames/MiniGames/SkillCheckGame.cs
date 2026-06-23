using MiniGames;
using MiniGames.MiniGames;
using PopupMiniGames.GameAssets.PolarEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            "GameAssets", "SkillCheckAssets"
        );

        private Game game = new Game();

        private int mistakes = 0;
        private int maxMistakes = 4;
        private int score = 0;
        private int maxScore = 10;
        private GameData gameData;

        private float speed;
        private int size;
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

            SkillCheckObject skillCheckObject = new SkillCheckObject(new Point(250, 250), game, speed, size, 0, Path.Combine(assetsPath, "SkillCheckPointer.png"), this);
            game.AddGameObject(skillCheckObject);
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
                    size = 50;
                    maxMistakes = 5;
                    speed = 150;
                    break;
                case Difficulty.Medium:
                    size = 40;
                    maxMistakes = 4;
                    speed = 200;
                    break;
                case Difficulty.Hard:
                    size = 25;
                    maxMistakes = 3;
                    speed = 300;
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



    internal class SkillCheckObject : GameObject
    {
        private float speed;
        private int size;
        private int startAngle;
        private Sprite skillCheckSprite;
        private Sprite pointerSprite;
        private float pointerRotation = 0;
        private int minSpace = 90;
        private int maxSpace = 150;
        private Random random = new Random();
        private SkillCheckGame skillCheckGame;

        public SkillCheckObject(Point position, Game game, float speed, int size, int startAngle, string pointerFilePath, SkillCheckGame skillCheckGame) : base(position, game)
        {
            this.skillCheckGame = skillCheckGame;
            this.size = size;
            this.speed = speed;
            this.startAngle = startAngle;

            skillCheckSprite = new Sprite(this, new Point(0, 0), null);
            UpdateSkillCheckSprite();
            AddComponent(skillCheckSprite);

            pointerSprite = new Sprite(this, Point.Empty, pointerFilePath);
            AddComponent(pointerSprite);
        }

        private void UpdateSkillCheckSprite()
        {
            using (Graphics g = Graphics.FromImage(skillCheckSprite.SpriteImage))
            {
                g.Clear(Color.Transparent);
                Rectangle rect = new Rectangle(25, 25, 450, 450);
                using (Pen pen = new Pen(Color.White, 25))
                {
                    g.DrawArc(pen, rect, startAngle, size);
                }
            }
        }
        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                int realPointerLocation = (int)pointerRotation - 90;
                realPointerLocation = realPointerLocation % 360;
                if ((realPointerLocation >= startAngle && realPointerLocation <= startAngle + size) ||
                    (realPointerLocation >= (startAngle + size) % 360 - size && realPointerLocation <= (startAngle + size) % 360))
                {
                    SpawnNewSkilCheck();
                    skillCheckGame.ChangeScore(1, 0);
                }
                else skillCheckGame.ChangeScore(0, 1);
            }

        }
        private void SpawnNewSkilCheck()
        {
            int spawnLocation = (int)pointerRotation - 90 + minSpace + random.Next(maxSpace);
            startAngle = spawnLocation % 360;

            UpdateSkillCheckSprite();
        }
        protected override void OnUpdate(float deltaTime)
        {
            pointerRotation += speed * deltaTime;
            pointerSprite.Rotation = (int)pointerRotation;
        }
    }
}
