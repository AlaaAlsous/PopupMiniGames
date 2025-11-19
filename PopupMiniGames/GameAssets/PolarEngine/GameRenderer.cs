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

        public GameRenderer()
        {
            Sprites = new List<Sprite>();
            BackgroundColor = Color.Black;
        }

        public void AddSprite(Sprite sprite) => Sprites.Add(sprite);
        public void RemoveSprite(Sprite sprite) => Sprites.Remove(sprite);
    }
}
