using Game;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.Scenes
{
    [CreateAssetMenu(fileName = "LevelPack", menuName = "SO/Scenes/newLevelPack")]
    public class LevelPackAsset : ScriptableObject
    {
        [field: SerializeField] public List<SceneField> levels;
    }
}
