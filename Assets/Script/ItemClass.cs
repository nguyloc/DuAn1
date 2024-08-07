
using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName ="New Item", menuName ="New Item")]
    public abstract class ItemClass : ScriptableObject
    {
        public string itemName;
        public Sprite itemIcon;

        public bool isStackable = true;

        public abstract ItemClass GetItem();
        public abstract ToolClass GetTool();
        public abstract MiscClass GetMiscClass();
        public abstract ConsumableClass GetConsumableClass();

    }
}
