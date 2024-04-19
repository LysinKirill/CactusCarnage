using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsAsset", menuName = "SO/Settings/newSettingsAsset")]
    public class SettingsAsset : ScriptableObject
    {
        [field: SerializeField] public GraphicsSettings graphicsLevel;
        [field: SerializeField, Range(0f, 1f)] public float masterVolume;
        [field: SerializeField, Range(0f, 1f)] public float musicVolume;
        [field: SerializeField, Range(0f, 1f)] public float sfxVolume;
        [field: SerializeField, Range(0f, 1.2f)] public float brightnessLevel;
    }

    public enum GraphicsSettings
    {
        Low,
        Medium,
        High,
    }
}
