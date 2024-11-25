using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] [FoldoutGroup("Effects")]
    private GameObject onDestroyedPrefab;

    private Level _level;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            var vfx = Instantiate(onDestroyedPrefab, transform.position, Quaternion.identity);

            if (vfx.TryGetComponent<ParticleSystem>(out var onDestroyedPrefabParticles))
            {
                var mainModule = onDestroyedPrefabParticles.main;
                mainModule.startColor = GetComponent<SpriteRenderer>().color;
            }


            gameObject.SetActive(false);
            EventController.OnBrickDestroyed?.Invoke();
        }
    }
}