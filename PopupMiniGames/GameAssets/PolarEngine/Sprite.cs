using System;
using System.Drawing;
using System.IO;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class Sprite
    {
        public Image SpriteImage { get; set; }
        public Point Position { get; set; }
        public int Rotation { get; set; }

        public Sprite(Point position, string? filePath = null, int rotation = 0)
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
    }
}
