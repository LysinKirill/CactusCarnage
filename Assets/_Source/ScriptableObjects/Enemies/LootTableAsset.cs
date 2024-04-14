using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "SO/Enemies/newLootTableAsset")]
    public class LootTableAsset : ScriptableObject
    {
        [field: SerializeField] public List<LootDescription> lootDescriptions;
    }

    [Serializable]
    public class LootDescription
    {
        public GameObject lootPrefab;
        [Range(0, 100)] public int dropChancePercentage;
    };
}
