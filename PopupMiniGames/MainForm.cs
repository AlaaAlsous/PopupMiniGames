using System.Windows.Forms;
namespace MiniGames
{
    public partial class MainForm : Form
    {
        private List<Type> miniGames = new List<Type>();
        private Random rand = new Random();
        private GameData gameData = new GameData();

        public MainForm()
        {
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeMenu();
            LoadMiniGames();
        }

        private void InitializeMenu()
        {
            Button difficultyButton = new Button();
            difficultyButton.Text = "Play";
            difficultyButton.Size = new Size(150, 40);
            difficultyButton.Location = new Point(70, 20);
            difficultyButton.Click += (s, e) => StartNextGame();

            Button exitButton = new Button();
            exitButton.Text = "Exit";
            exitButton.Size = new Size(150, 40);
            exitButton.Location = new Point(70, 80);
            exitButton.Click += (s, e) => Application.Exit();

            this.Controls.Add(difficultyButton);
            this.Controls.Add(exitButton);
        }
    }
}