using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }
}
