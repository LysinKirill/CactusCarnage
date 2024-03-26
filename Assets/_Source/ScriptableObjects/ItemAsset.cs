using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "SO/newItem")]
    public class ItemAsset : ScriptableObject
    {
        [field: SerializeField] public bool IsStackable { get; set; }
        [field: SerializeField] public int StackSize { get; set; } = 1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Sprite { get; set; }
    
        [field: SerializeField, TextArea] public string Description { get; set; }

        public int ID
        {
            get { return GetInstanceID(); }
        }
    }
}
