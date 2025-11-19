using System;
using System.Linq;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class GameObject
    {
        public Point Position { get; set; }
        public List<Component> Components { get; private set; }

        public GameObject(Point position)
        {
            Position = position;
            Components = new List<Component>();
        }
        public void AddComponent(Component component) => Components.Add(component);

    }
}
