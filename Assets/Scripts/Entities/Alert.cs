using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Entities
{
    public class Alert : MonoBehaviour
    {
        //Internal Timer for lifetime
        private float _timer;

        //Is the alert active/flashing?
        private bool _active;

        //The speed at which the prompt travels
        [FoldoutGroup("Options")] [SerializeField]
        private float speed;

        //How long the prompt should show 
        [FoldoutGroup("Options")] [SerializeField]
        private float lifeTime;

        //Fade Controls
        [FoldoutGroup("Components")] [SerializeField]
        private CanvasGroup cg;

        [FoldoutGroup("Options")] [SerializeField]
        private float fadeSpeed;

        //Text content of the Alert;
        [FoldoutGroup("Components")] [SerializeField]
        private TMP_Text text;

        //Internal tracking of the alpha of the alert
        private float colorAlpha;

        private void Start()
        {
            _timer = 0f;
            _active = true;
            colorAlpha = 1;
            StartCoroutine(Flash());
        }

        void Update()
        {
            //Movement
            transform.Translate(Vector3.up * (speed * Time.deltaTime));
            _timer += Time.deltaTime;

            //Lifetime Tracking - Set to Fadeout
            if (_timer >= lifeTime && _active)
            {
                _active = false;
                StopAllCoroutines();
                StartCoroutine(FadeOut());
            }
        }

        /// <summary>
        /// Sets the Text Content of the Alert
        /// </summary>
        /// <param name="newText">The content to display</param>
        public void SetText(string newText)
        {
            text.SetText(newText);
        }

        /// <summary>
        /// Set the colour of the Alert's text
        /// </summary>
        /// <param name="colour">The colour to display</param>
        public void SetColour(Color colour)
        {
            text.color = colour;
        }

        /// <summary>
        /// Flashes the Prompt
        /// </summary>
        /// <returns></returns>
        private IEnumerator Flash()
        {
            var direction = false;
            while (_active)
            {
                if (direction)
                {
                    colorAlpha -= fadeSpeed * Time.deltaTime;
                    if (colorAlpha <= 0.4) direction = false;
                }
                else
                {
                    colorAlpha += fadeSpeed * Time.deltaTime;
                    if (colorAlpha >= 1) direction = true;
                }

                cg.alpha = colorAlpha;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Fade the prompt out completely and then destroy it.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOut()
        {
            while (cg.alpha > 0)
            {
                cg.alpha = Mathf.MoveTowards(cg.alpha, 0, Time.deltaTime * fadeSpeed);
                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }
    }
}