using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public class Collider : Component
    {
        public Point position;
        public int width = 100;
        public int height = 100;
        public Game Game;

        public Collider(GameObject parentGameObject) : base(parentGameObject)
        { 
            Game = Parent.Game;
        }

        public Point GetTruePosition()
        {
            int x = position.X+Parent.Position.X;
            int y = position.Y+Parent.Position.Y;
            return new Point(x, y);
        }

        public bool Overlaps(Collider other)
        {
            Point realPos = GetTruePosition();
            Point otherRealPos = other.GetTruePosition();
            if (realPos.X + width <= otherRealPos.X) return false;
            if (realPos.X >= otherRealPos.X + other.width) return false;
            if (realPos.Y + height <= otherRealPos.Y) return false;
            if (realPos.Y >= otherRealPos.Y + other.height) return false;
            return true;
        }

    }
}
