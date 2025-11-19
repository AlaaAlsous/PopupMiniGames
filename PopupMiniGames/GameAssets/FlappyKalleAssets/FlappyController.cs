using MiniGames.MiniGames;
using PopupMiniGames.GameAssets.PolarEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopupMiniGames.GameAssets.FlappyKalleAssets
{
    internal class FlappyController : GameObject
    {
        private float velocity = 0;
        private float gravity = 30;
        private Sprite sprite;
        private Collider collider;

        private int flightStrength = 13;
        private FlappyKalle flappyKalle;
        public FlappyController(Point position, Game game, string imageFilePath, FlappyKalle flappyKalle) : base(position, game)
        {
            Position = new Point(100, 300);
            sprite = new Sprite(this, Point.Empty, imageFilePath);
            AddComponent(sprite);
            this.flappyKalle = flappyKalle;

            collider = new Collider(this);
            //sprite is 120x120, this gives 20 pixel room on each side that doesn't have collision
            collider.width = 80;
            collider.height = 80;
            collider.position = new Point(-40, -40);
            AddComponent(collider);
        }


        override protected void OnUpdate(float deltaTime)
        {
            velocity += gravity * deltaTime;

            int x = this.Position.X;
            int y = this.Position.Y + (int)velocity;
            y = Math.Clamp(y, 0, 900);
            this.Position = new Point(x, y);

            sprite.Rotation = Math.Clamp((int)velocity * 3, -50, 50);

            CheckCollided(Game.Colliders);
        }

        private void CheckCollided(List<Collider> others)
        {
            foreach (Collider other in others)
            {
                if (other == collider) continue;

                if (collider.Overlaps(other))
                {
                    flappyKalle.ChangeScore(0, 1);
                    Game.RemoveGameObject(other.Parent);
                }
            }
        }

        override public void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    velocity = -flightStrength;
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
            }

        }
    }
}
