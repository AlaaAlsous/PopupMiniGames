using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace PopupMiniGames.UI
{
    public class UIManager
    {
        private Form mainForm;
        private string basePath;

        public UIManager(Form form, string assetsPath)
        {
            mainForm = form;
            basePath = assetsPath;
            SetupForm();
        }

        private void SetupForm()
        {
            mainForm.Width = 800;
            mainForm.Height = 600;
            mainForm.Text = "Popup Mini Games";
            mainForm.FormBorderStyle = FormBorderStyle.None;
            mainForm.StartPosition = FormStartPosition.CenterScreen;

            mainForm.BackColor = Color.Black;
            mainForm.TransparencyKey = Color.Black;

            string bgPath = Path.Combine(basePath, "MENU_BG.png");
            if (File.Exists(bgPath))
            {
                mainForm.BackgroundImage = Image.FromFile(bgPath);
                mainForm.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }

        public Button CreateButton(string normalImage, string hoverImage, Point location, Action onClick)
        {
            Button btn = new Button();
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.BackColor = Color.Transparent;
            btn.TabStop = false;
            btn.Width = 200;
            btn.Height = 80;
            btn.Location = location;

            string normalPath = Path.Combine(basePath, normalImage);
            string hoverPath = Path.Combine(basePath, hoverImage);

            if (File.Exists(normalPath))
                btn.BackgroundImage = Image.FromFile(normalPath);
            btn.BackgroundImageLayout = ImageLayout.Stretch;

            btn.MouseEnter += (s, e) =>
            {
                if (File.Exists(hoverPath))
                    btn.BackgroundImage = Image.FromFile(hoverPath);
            };

            btn.MouseLeave += (s, e) =>
            {
                if (File.Exists(normalPath))
                    btn.BackgroundImage = Image.FromFile(normalPath);
            };

            btn.Click += (s, e) => onClick?.Invoke();

            mainForm.Controls.Add(btn);
            return btn;
        }
    }
}
