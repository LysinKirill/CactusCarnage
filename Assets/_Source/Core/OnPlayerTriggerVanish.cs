using UnityEngine;

namespace Core
{
    public class OnPlayerTriggerVanish : MonoBehaviour
    {
        [SerializeField] private float timeToVanish = 1;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            GetComponent<Animator>()?.SetTrigger("Vanish");
            GetComponent<AudioSource>()?.Play();
            Destroy(gameObject, 2);
        }
    }
}
