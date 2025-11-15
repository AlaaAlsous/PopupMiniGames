using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

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

            normalImagePath = Path.Combine(basePath, normalImage);
            selectedImagePath = Path.Combine(basePath, selectedImage);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            BackColor = Color.Transparent;
            TabStop = false;
            Width = 200;
            Height = 80;
            Location = location;
            BackgroundImageLayout = ImageLayout.Stretch;

            SetNormalState();
            Click += (s, e) => onClick?.Invoke();
        }

        public void SetSelectedState()
        {
            if (System.IO.File.Exists(selectedImagePath))
                this.BackgroundImage = Image.FromFile(selectedImagePath);
        }
        public void SetNormalState()
        {
            if (System.IO.File.Exists(normalImagePath))
                this.BackgroundImage = Image.FromFile(normalImagePath);
        }
    }
}
