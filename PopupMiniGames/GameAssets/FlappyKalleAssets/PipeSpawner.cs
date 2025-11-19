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

        private float timeSinceSpawn = 6;

        public PipeSpawner(Point position, Game game) :base(position, game)
        { 
            
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

        }
    }
}
