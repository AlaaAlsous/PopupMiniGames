using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopupMiniGames.UI
{
    internal class MenuButton : Button
    {
        private string selectedImagePath;
        private string normalImagePath;
        public MenuButton(string normalImage, string selectedImage, Point location, Action onClick, string basePath = "")
        {
            selectedImagePath = Path.Combine(basePath, selectedImage);
            normalImagePath = Path.Combine(basePath, normalImage);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.BackColor = Color.Transparent;
            this.TabStop = false;
            this.Width = 200;
            this.Height = 80;
            this.Location = location;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Click += (s, e) => onClick();
        }
    }
}
