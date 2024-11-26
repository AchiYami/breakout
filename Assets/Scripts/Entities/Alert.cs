using System.Collections;
using TMPro;
using UnityEngine;

public class Alert : MonoBehaviour
{
    private float _timer;
    private bool _active;

    [SerializeField] private float speed;

    [SerializeField] private float lifeTime;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private TMP_Text text;

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
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
        _timer += Time.deltaTime;
        
        if (_timer >= lifeTime && _active)
        {
            _active = false;
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }


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