using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class Game : IDisposable
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private Stopwatch stopwatch = new Stopwatch();
        public List<GameObject> GameObjects { get; protected set; } = new List<GameObject>();
        public List<Collider> Colliders { get; protected set; } = new List<Collider>();
        public GameRenderer Renderer { get; set; } = new GameRenderer();

        private List<GameObject> pendingRemovals = new List<GameObject>();
        private List<GameObject> pendingAdds = new List<GameObject>();

        private bool disposed = false;

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
            foreach (GameObject obj in GameObjects)
            {
                obj.Update(deltaTime);
            }

            ApplyPendingChanges();

            Renderer.UpdateFrame();
        }

        public void OnKeyDown(object? sender, KeyEventArgs e)
        {
            foreach (GameObject obj in GameObjects.ToArray())
            {
                obj.OnKeyDown(e);
            }
        }

        public void AddGameObject(GameObject obj)
        {
            if (obj == null) return;
            if (GameObjects.Contains(obj) || pendingAdds.Contains(obj)) return;
            if (pendingRemovals.Remove(obj)) return;

            obj.Game = this;
            pendingAdds.Add(obj);
        }

        public void RemoveGameObject(GameObject obj)
        {
            if (obj == null) return;

            if (pendingAdds.Remove(obj)) return;

            if (GameObjects.Contains(obj) && !pendingRemovals.Contains(obj))
            {
                pendingRemovals.Add(obj);
            }
        }

        private void ApplyPendingChanges()
        {
            if (pendingAdds.Count > 0)
            {
                var adds = pendingAdds.ToArray();
                pendingAdds.Clear();
                foreach (var obj in adds)
                {
                    if (obj == null) continue;
                    if (GameObjects.Contains(obj)) continue;

                    obj.Game = this;
                    GameObjects.Add(obj);

                    foreach (Sprite sprite in obj.Components.OfType<Sprite>()) Renderer.AddSprite(sprite);
                    foreach (Collider collider in obj.Components.OfType<Collider>())
                    {
                        Colliders.Add(collider);
                        collider.Game = this; //this should already be set, but why not do it again
                    }
                }
            }

            if (pendingRemovals.Count > 0)
            {
                var removes = pendingRemovals.ToArray();
                pendingRemovals.Clear();
                foreach (var obj in removes)
                {
                    if (obj == null) continue;

                    foreach (Sprite sprite in obj.Components.OfType<Sprite>()) Renderer.RemoveSprite(sprite);
                    foreach (Collider collider in obj.Components.OfType<Collider>())
                    {
                        Colliders.Remove(collider);
                        collider.Game = default!;
                    }
                    if (obj is IDisposable disposableObj) disposableObj.Dispose();

                    GameObjects.Remove(obj);
                }
            }
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            Stop();

            // dispose all managed objects
            foreach (var obj in GameObjects.ToArray())
            {
                if (obj is IDisposable d) d.Dispose();
            }
            GameObjects.Clear();
            Colliders.Clear();
            pendingAdds.Clear();
            pendingRemovals.Clear();

            Renderer.Dispose();
            timer.Dispose();
            Renderer?.Dispose();
            Renderer = default!;
        }
    }
}