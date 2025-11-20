using MiniGames.MiniGames;
using PopupMiniGames.GameAssets.PolarEngine;

namespace PopupMiniGames.GameAssets.FlappyKalleAssets
{
    internal class Pipe : GameObject
    {
        private FlappyKalle flappyKalle;
        private Collider collider;
        private Sprite sprite;
        private int speed;
        private float preciseX;

        private int width;
        private int height;
        public Pipe(Point position, int speed, Game game, int height, int width, FlappyKalle flappyKalle) : base(position, game)
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
            if (Game == null) return; //this shouldn't be necessary, but it is
            preciseX -= deltaTime * speed;
            Position = new Point((int)preciseX, Position.Y);

            if (preciseX < -100)
            {
                flappyKalle.ChangeScore(1, 0);
                Game.RemoveGameObject(this);
            }
        }
    }
}
