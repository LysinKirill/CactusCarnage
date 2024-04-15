using Core.Controllers;
using Core.Player;
using ScriptableObjects.Story;
using System;
using UnityEngine;

namespace Core.Environment
{
    public class StorylineTrigger : MonoBehaviour
    {
        [SerializeField] private StoryInsertionAsset storyInsertion;
        [SerializeField] private LayerMask playerLayer;

        public event Action<StoryInsertionAsset> OnTriggerActivated;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if((1 << other.gameObject.layer &  playerLayer) == 0
               || !other.TryGetComponent(out PlayerState _))
                return;
            OnTriggerActivated?.Invoke(storyInsertion);
            gameObject.SetActive(false);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
