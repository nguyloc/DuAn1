
using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName = "New Consum", menuName = "Item/Consumable")]
    public class ConsumableClass : ItemClass
    {
        [Header("Consumable")]
        public int healRecovery;

        public override ItemClass GetItem() { return this; }
        public override ToolClass GetTool() { return null; }
        public override MiscClass GetMiscClass() { return null; }
        public override ConsumableClass GetConsumableClass() { return this; }
    }
}
