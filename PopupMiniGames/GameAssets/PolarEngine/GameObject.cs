using System;
using System.Linq;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class GameObject
    {
        public Point Position { get; set; }
        public List<Component> Components { get; private set; }
        public Game Game { get; set; }
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


    }
}
