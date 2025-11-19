using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopupMiniGames.GameAssets.PolarEngine
{
    public abstract class Component
    {
        public GameObject Parent { get; set; }
        public void Update(float deltaTime) => OnUpdate(deltaTime);
        protected virtual void OnUpdate(float deltaTime) { }

        public Component(GameObject parent) { Parent = parent; }
    }
}
