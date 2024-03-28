using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ParallaxBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform followingTarget;
        [SerializeField, Range(0f, 1f)] private float parallaxStrength = 0.1f;
        [SerializeField] private bool disableVerticalParallax;
        private Vector3 targetPreviousPosition;
        
        private void Start()
        {
            if (followingTarget) return;
            if (Camera.main != null) followingTarget = Camera.main.transform;

            targetPreviousPosition = followingTarget.position;
        }

        private void FixedUpdate()
        {
            var delta = followingTarget.position - targetPreviousPosition;
            if (disableVerticalParallax) delta.y = 0f;
            targetPreviousPosition = followingTarget.position;
            transform.position += delta * parallaxStrength;
        }
    }
}
