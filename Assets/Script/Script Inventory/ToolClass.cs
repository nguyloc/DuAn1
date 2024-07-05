using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tool", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    public int toolDamage;
    public int toolDurability;
    public override ItemClass GetItem() {  return this; }
    public override ToolClass GetTool() { return this; }
    public override MiscClass GetMiscClass() { return null; }
    public override ConsumableClass GetConsumableClass() { return null; }
}
