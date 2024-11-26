using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "SpawnOnTimer", menuName = "EasterEgg/SpawnOnTimer")]
    public class SpawnOnTimer : EasterEgg
    {
        [SerializeField] private GameObject objectToSpawn;
        [SerializeField] private float lifeTime;

        [SerializeField] private Transform parent;

        protected override void Activate()
        {
            isActive = true;

            var newObject = Instantiate(objectToSpawn, parent);
            Destroy(newObject, lifeTime);
        }

        protected override void Deactivate()
        {
            isActive = false;
        }
    }
}