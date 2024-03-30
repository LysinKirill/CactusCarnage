using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class HeartVisuals : MonoBehaviour
    {
        [SerializeField] private GameObject heart1;
        [SerializeField] private GameObject heart2;
        [SerializeField] private GameObject heart3;
        [FormerlySerializedAs("playerData")]
        [SerializeField] private PlayerHealth playerHealth;

        void Start()
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }

        void Update()
        {
            if (playerHealth.HealthPoints <= 0)
            {
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }
            if (playerHealth.HealthPoints == 1)
            {
                heart1.SetActive(true);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }
            if (playerHealth.HealthPoints == 2)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(false);
            }
            if (playerHealth.HealthPoints == 3)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
            }
        }
    }
}

