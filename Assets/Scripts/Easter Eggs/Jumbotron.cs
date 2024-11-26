using TMPro;
using UnityEngine;

namespace Utilities
{
    public class Jumbotron : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private TMP_Text text;
        [SerializeField] private bool isActive;
        [SerializeField] private float lifeTime;

        private void Start()
        {
            isActive = true;
            Invoke("Destroy", lifeTime);
        }

        public void SetData(string textContent, float duration, float newSpeed)
        {
            text.SetText(textContent);
            lifeTime = duration;
            speed = newSpeed;
        }

        private void Update()
        {
            if (!isActive) return;

            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}