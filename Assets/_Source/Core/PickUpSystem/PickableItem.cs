using ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Core.PickUpSystem
{
    public class PickableItem : MonoBehaviour
    {
        [field: SerializeField] public ItemAsset Item { get; private set; }
        [field: SerializeField] public int Quantity { get; set; } = 1;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float duration = 0.3f;


        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = Item.Sprite;
        }

        public void DestroyItem()
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(AnimateItemPickup());
        }

        private IEnumerator AnimateItemPickup()
        {
            audioSource.Play();
            var startScale = transform.localScale;
            var endScale = Vector3.zero;
            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
