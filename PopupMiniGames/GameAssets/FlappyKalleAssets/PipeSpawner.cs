using MiniGames.MiniGames;
using PopupMiniGames.GameAssets.PolarEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopupMiniGames.GameAssets.FlappyKalleAssets
{
    internal class PipeSpawner : GameObject
    {
        public float TimeBetweenSpawns { get; set; } = 6;
        public int MaxSpawnY { get; set; } = 100;
        public int MinSpawnY { get; set; } = -300;
        public int SpawnX { get; set; } = 1000;
        public int MiddleSpace { get; set; } = 300;
        public int Speed { get; set; } = 120;

        private float timeSinceSpawn = 6;
        private int height = 800;
        private int width = 100;
        private FlappyKalle flappyKalle;

        private Random random = new Random();
        public PipeSpawner(Point position, Game game, FlappyKalle flappyKalle) :base(position, game)
        { 
            this.flappyKalle = flappyKalle;
        }

        protected override void OnUpdate(float deltaTime)
        {
            timeSinceSpawn += deltaTime;
            if (timeSinceSpawn > TimeBetweenSpawns)
            { 

                SpawnPipe();
            }
        }

        private void SpawnPipe()
        {
            timeSinceSpawn = 0;
            int y = random.Next(MinSpawnY, MaxSpawnY);

            Point bottomSpawn = new Point(SpawnX, y);
            Point upperSpawn = new Point(SpawnX, y+MiddleSpace+height);
            Pipe bottomPipe = new Pipe(bottomSpawn, Speed, Game, height, width, flappyKalle);
            Game.AddGameObject(bottomPipe);
            Pipe upperPipe = new Pipe(upperSpawn, Speed, Game, height, width, flappyKalle);
            Game.AddGameObject(upperPipe);

        }
    }
}
