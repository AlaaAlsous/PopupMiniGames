using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class GameRenderer : PictureBox
    {
        public List<Sprite> Sprites { get; private set; }
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)] //ingen aning vad det betyder, varför det behövs, men fixade rött ._.
        public Color BackgroundColor { get; set; }
        private Bitmap canvas;

        public GameRenderer()
        {
            Sprites = new List<Sprite>();
            BackgroundColor = Color.Black;
            canvas = new Bitmap(Width, Height);
        }
        public void UpdateFrame()
        {
            if (canvas.Width != Width || canvas.Height != Height)
            {
                canvas = new Bitmap(Width, Height);
            }
            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.Clear(BackgroundColor);
                for (int i = 0; i < Sprites.Count; i++)
                {
                    Sprite s = Sprites[i];
                    Point pos = s.Position;

                    g.TranslateTransform(pos.X, pos.Y);
                    g.RotateTransform(s.Rotation);
                    g.DrawImage(s.SpriteImage, -s.SpriteImage.Width / 2, -s.SpriteImage.Height / 2);
                    g.ResetTransform();
                }
                Image = canvas;
            }
        }

        public void AddSprite(Sprite sprite) => Sprites.Add(sprite);
        public void RemoveSprite(Sprite sprite) => Sprites.Remove(sprite);
    }
}
