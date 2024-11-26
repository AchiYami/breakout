using Sirenix.OdinInspector;
using UnityEngine;

public class Brick : MonoBehaviour
{
    //Prefab to spawn on the brick being destroyed
    [SerializeField] [FoldoutGroup("Effects")]
    private GameObject onDestroyedPrefab;

    /// <summary>
    /// On Collision detected Logic - Destroy Brick, Update Score & Play Sound
    /// </summary>
    /// <param name="other">The source of the collision</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        //If it's the ball
        if (other.transform.CompareTag("Ball"))
        {
            //Spawn the VFX object
            var vfx = Instantiate(onDestroyedPrefab, transform.position, Quaternion.identity);

            //If it is particles, match the colour to the brick's colour.
            if (vfx.TryGetComponent<ParticleSystem>(out var onDestroyedPrefabParticles))
            {
                var mainModule = onDestroyedPrefabParticles.main;
                mainModule.startColor = GetComponent<SpriteRenderer>().color;
            }
            
            //'Destroy' the brick, and call any events to do with brick destruction
            gameObject.SetActive(false);
            EventController.OnBrickDestroyed?.Invoke();
        }
    }
}