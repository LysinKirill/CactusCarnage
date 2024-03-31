using Core.Player;
using System;
using UnityEngine;

namespace Core.Environment
{
    public class Void : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerHealth playerHealth))
                return;
            playerHealth.TakeDamage(Int32.MaxValue);
        }
    }
}
