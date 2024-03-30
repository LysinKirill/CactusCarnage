using UnityEngine;

namespace Model.ItemParameters
{
    [CreateAssetMenu(fileName = "ItemParameter", menuName = "SO/newItemParameter")]
    public class ItemParameterAsset : ScriptableObject
    {
        [field: SerializeField]
        public string ParameterName { get; private set; }
    }
}
