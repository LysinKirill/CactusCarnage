using UnityEngine;

namespace ScriptableObjects.Story
{
    [CreateAssetMenu(fileName = "StoryInsertion", menuName = "SO/Story/newStoryInsertion")]
    public class StoryInsertionAsset : ScriptableObject
    {
        [field: SerializeField] public bool IsRepeatable { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public GameObject storyPanelPrefab { get; private set; }
        [field: SerializeField] public float FadeOutDuration { get; private set; }
        [field: SerializeField] public string Text { get; private set; }
    }
}
