using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MiniGames.UI
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

            SetBackground("MENU_BG.png");
        }
        public void SetBackground(string imageName)
        {
            string path = Path.Combine(basePath, imageName);
            if (File.Exists(path))
            {
                mainForm.BackgroundImage = Image.FromFile(path);
                mainForm.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }

        public Button CreateButton(string normalImage, string hoverImage, Point location, Action onClick)
        {
            Button btn = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                TabStop = false,
                Width = 200,
                Height = 80,
                Location = location,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;

            string normalPath = Path.Combine(basePath, normalImage);
            string hoverPath = Path.Combine(basePath, hoverImage);

            btn.BackgroundImage = LoadImage(normalPath);

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
        private Image LoadImage(string path, string fallbackPath = "")
        {
            if (File.Exists(path))
                return Image.FromFile(path);
            return null!;
        }
    }
}
