using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "BigDog", menuName = "EasterEgg/BigDog")]
    public class BigDog : EasterEgg
    {
        [SerializeField] private Ball ball;
        [SerializeField] private Sprite ballNormalImage;
        [SerializeField] private Sprite ballEggImage;

        public override void Initialize()
        {
            isActive = false;
        }

        protected override void Activate()
        {
            if (ball == null)
            {
                ball = FindFirstObjectByType<Ball>();
            }

            isActive = true;
            var collider = ball.GetComponent<CircleCollider2D>();
            collider.radius = 2.5f;

            var renderer = ball.GetComponent<SpriteRenderer>();
            renderer.sprite = ballEggImage;
        }

        protected override void Deactivate()
        {
            if (ball == null)
            {
                ball = FindFirstObjectByType<Ball>();
            }

            isActive = false;
            var collider = ball.GetComponent<CircleCollider2D>();
            collider.radius = 0.65f;

            var renderer = ball.GetComponent<SpriteRenderer>();
            renderer.sprite = ballNormalImage;
        }
    }
}