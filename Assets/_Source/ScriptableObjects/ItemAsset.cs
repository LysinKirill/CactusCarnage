using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class ItemAsset : ScriptableObject
    {
        [field: SerializeField] public bool IsStackable { get; set; }
        [field: SerializeField] public int StackSize { get; set; } = 1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField, TextArea] public string Description { get; set; }
        [field: SerializeField] public List<ItemParameter> DefaultParameterList { get; set; }

        public int ID
        {
            get { return GetInstanceID(); }
        }
    }

}
