using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class Game
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private Stopwatch stopwatch = new Stopwatch();
        public List<GameObject> GameObjects { get; protected set; } = new List<GameObject>();
        public GameRenderer Renderer { get; set; } = new GameRenderer();

        public Game() { }

        public void Start()
        {
            timer.Interval = 16; //60-ish FPS
            timer.Tick += Update;
            timer.Start();
            stopwatch.Start();
        }

        public void Stop()
        {
            timer.Stop();
            timer.Tick -= Update;
            stopwatch.Reset();
        }

        public void Update(object? sender, EventArgs e)
        {
            float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();

            Renderer.UpdateFrame();
        }
    }
}