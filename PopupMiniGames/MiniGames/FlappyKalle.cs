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
                    break;
                case Difficulty.Medium:
                    maxMistakes = 3;
                    maxScore = 14;
                    pipeSpawner.TimeBetweenSpawns = 2.6f;
                    pipeSpawner.Speed = 200;
                    pipeSpawner.MiddleSpace = 260;
                    break;
                case Difficulty.Hard:
                    maxMistakes = 3;
                    maxScore = 18;
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
                ? $"You won!"
                : $"You lost!";
            MessageBox.Show(resultMessage, "Game Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

    internal class FlappyController : GameObject
    {
        private float velocity = 0;
        private float gravity = 30;
        private Sprite sprite;
        private Collider collider;

        private int flightStrength = 13;
        private FlappyKalle flappyKalle;
        public FlappyController(Point position, Game game, string imageFilePath, FlappyKalle flappyKalle) : base(position, game)
        {
            Position = new Point(100, 300);
            sprite = new Sprite(this, Point.Empty, imageFilePath);
            AddComponent(sprite);
            this.flappyKalle = flappyKalle;

            collider = new Collider(this);
            //sprite is 120x120, this gives 20 pixel room on each side that doesn't have collision
            collider.width = 80;
            collider.height = 80;
            collider.position = new Point(-40, -40);
            AddComponent(collider);
        }


        override protected void OnUpdate(float deltaTime)
        {
            velocity += gravity * deltaTime;

            int x = this.Position.X;
            int y = this.Position.Y + (int)velocity;
            y = Math.Clamp(y, 0, 900);
            this.Position = new Point(x, y);

            sprite.Rotation = Math.Clamp((int)velocity * 3, -50, 50);

            CheckCollided(Game.Colliders);
        }

        private void CheckCollided(List<Collider> others)
        {
            foreach (Collider other in others)
            {
                if (other == collider) continue;

                if (collider.Overlaps(other))
                {
                    flappyKalle.ChangeScore(0, 1);
                    Game.RemoveGameObject(other.Parent);
                }
            }
        }

        override public void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    velocity = -flightStrength;
                    break;
            }

        }
    }


    internal class Pipe : GameObject
    {
        private FlappyKalle flappyKalle;
        private Collider collider;
        private Sprite sprite;
        private int speed;
        private float preciseX;

        private int width;
        private int height;

        private bool isDeleted = false; // <-- Fix: så vi inte tar bort två gånger

        public Pipe(Point position, int speed, Game game, int height, int width, FlappyKalle flappyKalle)
            : base(position, game)
        {
            preciseX = position.X;
            sprite = new Sprite(this, Point.Empty);
            this.speed = speed;
            this.width = width;
            this.height = height;
            this.flappyKalle = flappyKalle;

            var old = sprite.SpriteImage;
            sprite.SpriteImage = GetBitmap();
            old?.Dispose();

            AddComponent(sprite);

            collider = new Collider(this);
            collider.height = height;
            collider.width = width;
            collider.position = new Point(-width / 2, -height / 2);
            AddComponent(collider);
        }

        private Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Green);
            }
            return bitmap;
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Skydda mot null direkt
            if (Game == null || flappyKalle == null)
                return;

            preciseX -= deltaTime * speed;
            Position = new Point((int)preciseX, Position.Y);

            if (preciseX < -100)
            {
                flappyKalle.ChangeScore(1, 0);

                // Kontrollera att objektet fortfarande finns i spelet
                if (Game.GameObjects.Contains(this))
                {
                    Game.RemoveGameObject(this);
                }
            }
        }

    }


    internal class PipeSpawner : GameObject
    {
        public float TimeBetweenSpawns { get; set; } = 6;
        public int MaxSpawnY { get; set; } = 100;
        public int MinSpawnY { get; set; } = -300;
        public int SpawnX { get; set; } = 1000;
        public int MiddleSpace { get; set; } = 300;
        public int Speed { get; set; } = 120;

        private float timeSinceSpawn = 6;
        private int height = 800;
        private int width = 100;
        private FlappyKalle flappyKalle;

        private Random random = new Random();
        public PipeSpawner(Point position, Game game, FlappyKalle flappyKalle) : base(position, game)
        {
            this.flappyKalle = flappyKalle;
        }

        protected override void OnUpdate(float deltaTime)
        {
            timeSinceSpawn += deltaTime;
            if (timeSinceSpawn > TimeBetweenSpawns)
            {

                SpawnPipe();
            }
        }

        private void SpawnPipe()
        {
            timeSinceSpawn = 0;
            int y = random.Next(MinSpawnY, MaxSpawnY);

            Point bottomSpawn = new Point(SpawnX, y);
            Point upperSpawn = new Point(SpawnX, y + MiddleSpace + height);
            Pipe bottomPipe = new Pipe(bottomSpawn, Speed, Game, height, width, flappyKalle);
            Game.AddGameObject(bottomPipe);
            Pipe upperPipe = new Pipe(upperSpawn, Speed, Game, height, width, flappyKalle);
            Game.AddGameObject(upperPipe);

        }
    }
}
