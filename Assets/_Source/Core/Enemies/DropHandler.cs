using ScriptableObjects.Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Core.Enemies
{
    public class DropHandler : MonoBehaviour
    {
        [SerializeField] private GameObject pickups;
        [SerializeField] private LootTableAsset lootTable;

        private readonly Random _random = new Random();

        private void Awake()
        {
            if (!TryGetComponent(out EnemyState health))
                return;

            health.OnDeath += AttemptToGenerateLoot;
        }

        private void AttemptToGenerateLoot(GameObject enemy)
        {
            List<GameObject> prefabsToInstantiate = new List<GameObject>();
            foreach (var lootDescription in lootTable.lootDescriptions)
            {
                var randValue = _random.Next(0, 101);
                if (randValue <= lootDescription.dropChancePercentage)
                    prefabsToInstantiate.Add(lootDescription.lootPrefab);
            }

            foreach (GameObject prefab in prefabsToInstantiate)
            {
                var droppedItem = Instantiate(
                    prefab,
                    enemy.transform.position,
                    Quaternion.identity
                );
                if (pickups != null)
                    droppedItem.transform.SetParent(pickups.transform);
            }
        }
    }
}
