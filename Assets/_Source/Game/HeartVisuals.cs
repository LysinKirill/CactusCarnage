using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Game
{
    public class HeartVisuals : MonoBehaviour
    {
        [SerializeField] private GameObject heart1;
        [SerializeField] private GameObject heart2;
        [SerializeField] private GameObject heart3;
        [SerializeField] private PlayerData playerData;

        void Start()
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }

        void Update()
        {
            if (playerData.GetHealthPoints() <= 0)
            {
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }
            if (playerData.GetHealthPoints() == 1)
            {
                heart1.SetActive(true);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }
            if (playerData.GetHealthPoints() == 2)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(false);
            }
            if (playerData.GetHealthPoints() == 3)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
            }
        }
    }
}

