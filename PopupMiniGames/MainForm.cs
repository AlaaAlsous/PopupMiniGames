using PopupMiniGames.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MiniGames.MiniGames;
using MiniGames.UI;

namespace MiniGames
{
    public partial class MainForm : Form
    {
        private List<Type> miniGames = new List<Type>();
        private Random rand = new Random();
        private GameData gameData = new GameData();

        private PictureBox difficultyIcon;
        private UIManager uiManager;
        private Menu mainMenu;

        public MainForm()
        {
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            gameData.SetDifficulty(Difficulty.Medium);

            string basePath = Path.Combine(Application.StartupPath, @"..\..\..\UIAssets");
            uiManager = new UIManager(this, basePath);

            SetupMenu();
            LoadMiniGames();
        }

        private void SetupMenu()
        {
            // Bakgrund
            uiManager.SetBackground("MENU_BG.png");

            // Knappar
            string basePath = Path.Combine(Application.StartupPath, @"..\..\..\UIAssets");
            mainMenu = new Menu(this);
            mainMenu.AddMenuButton(new MenuButton("PLAY.png", "PLAYPRESS.png", new Point(310, 100), StartNextGame, basePath));
            mainMenu.AddMenuButton(new MenuButton("OPTIONS.png", "OPTIONSPRESS.png", new Point(310, 200), ShowOptions, basePath));
            mainMenu.AddMenuButton(new MenuButton("EXIT.png", "EXITPRESS.png", new Point(310, 300), () => Application.Exit(), basePath));
            this.KeyDown += mainMenu.OnKeyDown!;
            mainMenu.onBack += Application.Exit;

            currentDifficultyIcon();
        }

        private void currentDifficultyIcon()
        {
            if (difficultyIcon != null)
            {
                this.Controls.Remove(difficultyIcon);
                difficultyIcon.Dispose();
            }

            difficultyIcon = new PictureBox
            {
                Location = new Point(235, 400),
                Size = new Size(350, 150),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            string img = gameData.Difficulty switch
            {
                Difficulty.Easy => "SL_EASY.png",
                Difficulty.Medium => "SL_MEDIUM.png",
                Difficulty.Hard => "SL_HARD.png",
                _ => "SL_EASY.png"
            };

            string path = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\UIAssets", img);
            difficultyIcon.Image = Image.FromFile(path);

            this.Controls.Add(difficultyIcon);
            difficultyIcon.BringToFront();
        }

        private void ShowOptions()
        {
            Form optionsForm = new Form
            {
                Width = 800,
                Height = 600,
                Text = "Options",
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.Black,
                TransparencyKey = Color.Black,
                ShowInTaskbar = false,
                Owner = this
            };

            string basePath = System.IO.Path.Combine(Application.StartupPath, @"..\..\..\UIAssets");
            Menu optionsMenu = new Menu(optionsForm);
            optionsForm.KeyPreview = true;
            optionsForm.KeyDown += optionsMenu.OnKeyDown!;
            optionsMenu.onBack += optionsForm.Close;

            optionsMenu.AddMenuButton(new MenuButton("EASY.png", "EASYPRESS.png", new Point(310, 100), () =>
            {
                gameData.SetDifficulty(Difficulty.Easy);
                currentDifficultyIcon();
                optionsForm.Close();
            }, basePath));
            optionsMenu.AddMenuButton(new MenuButton("MEDIUM.png", "MEDIUMPRESS.png", new Point(310, 200), () =>
            {
                gameData.SetDifficulty(Difficulty.Medium);
                currentDifficultyIcon();
                optionsForm.Close();
            }, basePath));
            optionsMenu.AddMenuButton(new MenuButton("HARD.png", "HARDPRESS.png", new Point(310, 300), () =>
            {
                gameData.SetDifficulty(Difficulty.Hard);
                currentDifficultyIcon();
                optionsForm.Close();
            }, basePath));

            optionsForm.ShowDialog();
        }

        private void StartNextGame()
        {
            // “Kill” menyn helt
            if (mainMenu != null)
            {
                foreach (var btn in mainMenu.Buttons)
                {
                    this.Controls.Remove(btn);
                    btn.Dispose();
                }
                mainMenu = null;
            }

            if (difficultyIcon != null)
            {
                this.Controls.Remove(difficultyIcon);
                difficultyIcon.Dispose();
                difficultyIcon = null;
            }

            this.BackgroundImage = null;

            // Kontrollera game-over / win
            if (gameData.Score >= gameData.MaxScore)
            {
                MessageBox.Show("You Win!");
                gameData.Score = 0;
                gameData.Mistakes = 0;
                SetupMenu();
                return;
            }

            if (gameData.Mistakes >= gameData.MaxMistakes)
            {
                MessageBox.Show("Game Over!");
                gameData.Score = 0;
                gameData.Mistakes = 0;
                SetupMenu();
                return;
            }

            if (miniGames.Count == 0)
            {
                MessageBox.Show("No mini-games have been added yet!");
                SetupMenu();
                return;
            }

            int index = rand.Next(miniGames.Count);
            var instance = Activator.CreateInstance(miniGames[index]) as IMiniGame;
            if (instance != null)
            {
                instance.GameEnded += OnMiniGameEnded!;
                instance.StartGame(gameData.Difficulty);
            }
            else
            {
                MessageBox.Show($"Could not create instance of {miniGames[index].Name}");
                SetupMenu();
            }
        }
        private void LoadMiniGames()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && typeof(IMiniGame).IsAssignableFrom(type) && type.Namespace == "MiniGames.MiniGames")
                {
                    miniGames.Add(type);
                }
            }
        }
        private void OnMiniGameEnded(object sender, GameResult e)
        {
            gameData.Score += e.Points;
            gameData.Mistakes += e.Mistakes;
            StartNextGame();
        }
    }
}
