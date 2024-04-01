using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Items
{
    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip ActionSfx { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }
}
