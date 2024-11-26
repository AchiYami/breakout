using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "Jumbo", menuName = "EasterEgg/Jumbo")]
    public class Jumbo : EasterEgg
    {
        [SerializeField] private GameObject jumboPrefab;
        [SerializeField] private GameObject jumboStartPoint;
        [SerializeField] private float lifeTime;
        [SerializeField] private string text;
        [SerializeField] private float speed;

        private Jumbotron _currentJumbo;

        protected override void Activate()
        {
            if (_currentJumbo != null)
            {
                Destroy(_currentJumbo.gameObject);
            }

            isActive = true;

            _currentJumbo = Instantiate(jumboPrefab, jumboStartPoint.transform).GetComponent<Jumbotron>();
            _currentJumbo.SetData(text, lifeTime, speed);
        }

        protected override void Deactivate()
        {
            if (_currentJumbo != null)
            {
                Destroy(_currentJumbo.gameObject);
            }

            isActive = false;
        }
    }
}