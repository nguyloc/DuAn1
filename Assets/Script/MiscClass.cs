
using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName = "New Misc", menuName = "Item/Misc")]
    public class MiscClass : ItemClass
    {
        [Header("Misc")]
        public string itemDecription;

        public override ItemClass GetItem() { return this; }
        public override ToolClass GetTool() { return null; }
        public override MiscClass GetMiscClass() { return this; }
        public override ConsumableClass GetConsumableClass() { return null; }
    }
}
