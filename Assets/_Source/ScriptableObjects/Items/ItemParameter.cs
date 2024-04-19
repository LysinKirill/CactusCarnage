using Model.ItemParameters;
using System;

namespace ScriptableObjects.Items
{
    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterAsset itemParameter;
        public float value;
        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemParameter other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)value * itemParameter.ParameterName.GetHashCode();
        }
    }
}
