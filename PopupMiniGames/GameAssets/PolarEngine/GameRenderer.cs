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

        private bool disposed;
        public GameRenderer()
        {
            Sprites = new List<Sprite>();
            BackgroundColor = Color.Black;
            canvas = new Bitmap(Width, Height);
        }

        public void UpdateFrame()
        {
            if (disposed) return;
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
                    Point pos = s.GetTruePosition();

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

        protected override void Dispose(bool disposing)
        {
            disposed = true;
            if (disposing)
            {
                // Ensure the PictureBox image is cleared and disposed
                var img = base.Image;
                if (img != null)
                {
                    base.Image = null;
                    img.Dispose();
                }

                canvas?.Dispose();
                canvas = null;
            }
            base.Dispose(disposing);
        }
    }
}
