using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(menuName = "EasterEgg")]
    public abstract class EasterEgg : ScriptableObject
    {
        public string displayName;
        public string triggerCode;
        public List<GameObject> dependencies;
        public bool isActive;
        
        public void Trigger()
        {
            if (isActive)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }

        public abstract void Initialize();
        protected abstract void Activate();
        protected abstract void Deactivate();
    }
}