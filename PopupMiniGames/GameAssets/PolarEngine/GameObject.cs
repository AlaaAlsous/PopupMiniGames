using System;
using System.Linq;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class GameObject : IDisposable
    {
        public Point Position { get; set; }
        public List<Component> Components { get; private set; }
        public Game Game { get; set; }

        private bool disposed = false;

        public GameObject(Point position, Game game)
        {
            Game = game;
            Position = position;
            Components = new List<Component>();
        }
        public void AddComponent(Component component) => Components.Add(component);
        public void Update(float deltaTime)
        {
            foreach (var component in Components)
            {
                component.Update(deltaTime);
            }
            OnUpdate(deltaTime);
        }
        protected virtual void OnUpdate(float deltaTime) { }
        public virtual void OnKeyDown(KeyEventArgs e) { }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            foreach (var c in Components.OfType<IDisposable>())
            {
                c.Dispose();
            }

            Components.Clear();
            Game = null;
        }
    }
}
