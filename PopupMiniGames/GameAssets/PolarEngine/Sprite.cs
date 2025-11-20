using System;
using System.Drawing;
using System.IO;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class Sprite : Component, IDisposable
    {
        public Image SpriteImage { get; set; }
        public Point Position { get; set; }
        public int Rotation { get; set; }

        private bool disposed = false;

        public Sprite(GameObject parentObject, Point position, string? filePath = null, int rotation = 0) : base(parentObject)
        {
            Position = position;
            Rotation = rotation;

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                SpriteImage = Image.FromFile(filePath);
            }
            else
            {
                Bitmap bmp = new Bitmap(500, 500);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                }
                SpriteImage = bmp;
            }
        }

        public Point GetTruePosition()
        {
            int x = Position.X + Parent.Position.X;
            int y = Position.Y + Parent.Position.Y;
            return new Point(x, y);
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            SpriteImage?.Dispose();
            SpriteImage = default!;
        }
    }
}