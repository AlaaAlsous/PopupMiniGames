using PopupMiniGames.UI;
using MiniGames.MiniGames;
using MiniGames.UI;

namespace MiniGames
{
    public partial class MainForm : Form
    {
        private List<Type> miniGames = new List<Type>();
        private List<Type> remainingGames = new List<Type>();
        private Random rand = new Random();
        private GameData gameData = new GameData();
        private PictureBox? difficultyIcon;
        private UIManager uiManager;
        private Menu? mainMenu;

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
            uiManager.SetBackground("MENU_BG.png");

            string basePath = Path.Combine(Application.StartupPath, @"..\..\..\UIAssets");
            mainMenu = new Menu(this);
            mainMenu.AddMenuButton(new MenuButton("PLAY.png", "PLAYPRESS.png", new Point(310, 100), StartNextGame, basePath));
            mainMenu.AddMenuButton(new MenuButton("OPTIONS.png", "OPTIONSPRESS.png", new Point(310, 200), ShowOptions, basePath));
            mainMenu.AddMenuButton(new MenuButton("EXIT.png", "EXITPRESS.png", new Point(310, 300), () => Application.Exit(), basePath));
            this.KeyDown += mainMenu.OnKeyDown!;
            mainMenu.OnBack += Application.Exit;

            CurrentDifficultyIcon();
        }

        private void CurrentDifficultyIcon()
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
            optionsMenu.OnBack += optionsForm.Close;

            optionsMenu.AddMenuButton(new MenuButton("EASY.png", "EASYPRESS.png", new Point(310, 100), () =>
            {
                gameData.SetDifficulty(Difficulty.Easy);
                CurrentDifficultyIcon();
                optionsForm.Close();
            }, basePath));
            optionsMenu.AddMenuButton(new MenuButton("MEDIUM.png", "MEDIUMPRESS.png", new Point(310, 200), () =>
            {
                gameData.SetDifficulty(Difficulty.Medium);
                CurrentDifficultyIcon();
                optionsForm.Close();
            }, basePath));
            optionsMenu.AddMenuButton(new MenuButton("HARD.png", "HARDPRESS.png", new Point(310, 300), () =>
            {
                gameData.SetDifficulty(Difficulty.Hard);
                CurrentDifficultyIcon();
                optionsForm.Close();
            }, basePath));

            optionsForm.ShowDialog();
        }

        private void StartNextGame()
        {
            if (mainMenu != null)
            {
                mainMenu.ClearButtons(this);
                mainMenu = null;
            }

            if (difficultyIcon != null)
            {
                this.Controls.Remove(difficultyIcon);
                difficultyIcon.Dispose();
                difficultyIcon = null;
            }

            this.BackgroundImage = null;

            if (gameData.Score >= gameData.MaxScore)
            {
                MessageBox.Show("You Won Popup Mini Games!");
                gameData.Score = 0;
                gameData.Mistakes = 0;
                SetupMenu();
                return;
            }

            if (gameData.Mistakes >= gameData.MaxMistakes)
            {
                MessageBox.Show("Game Over! You lost Popup Mini Games!");
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

            if (remainingGames.Count == 0)
                remainingGames = new List<Type>(miniGames);

            int index = rand.Next(remainingGames.Count);
            var selectedGame = remainingGames[index];
            remainingGames.RemoveAt(index);

            IMiniGame? instance;

            if (selectedGame == typeof(FindMatchGame))
            {
                instance = (IMiniGame)Activator.CreateInstance(selectedGame, this, gameData)!;
            }
            else
            {
                instance = (IMiniGame)Activator.CreateInstance(selectedGame, gameData)!;
            }

            if (instance != null)
            {
                instance.GameEnded += OnMiniGameEnded!;
                instance.StartGame(gameData.Difficulty);
            }
            else
            {
                MessageBox.Show($"Could not create instance of {selectedGame.Name}");
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
            remainingGames = new List<Type>(miniGames);
        }
        private void OnMiniGameEnded(object sender, GameResult e)
        {
            if (sender is IMiniGameWithCleanup game)
            {
                game.Cleanup();
            }
            gameData.Score += e.Points;
            gameData.Mistakes += e.Mistakes;
            if (e.Won)
            {
                MessageBox.Show(
                    $"Well Done!\nYou won {e.GameName}!\n\n" +
                    $"Your Score: {gameData.Score} - Max Score: {gameData.MaxScore}\n" +
                    $"Your Mistakes: {gameData.Mistakes} - Max Mistakes: {gameData.MaxMistakes}"
                );
            }
            else
            {
                MessageBox.Show(
                    $"Game over! You lost {e.GameName}!\n\n" +
                    $"Your Score: {gameData.Score} - Max Score: {gameData.MaxScore}\n" +
                    $"Your Mistakes: {gameData.Mistakes} - Max Mistakes: {gameData.MaxMistakes}"
                );
            }
            this.BackgroundImage = null;
            SetupMenu();

            StartNextGame();
        }
    }
}
