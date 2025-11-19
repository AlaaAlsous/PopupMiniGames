using MiniGames.MiniGames;
using PopupMiniGames.GameAssets.PolarEngine;

namespace PopupMiniGames.GameAssets.FlappyKalleAssets
{
    internal class Pipe : GameObject
    {
        Collider collider;
        Sprite sprite;

        int width;
        int height;
        public Pipe(Point position, Game game, int height, int width) : base(position, game)
        {
            sprite = new Sprite(this, Point.Empty);
            this.width = width;
            this.height = height;

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

    }
}
