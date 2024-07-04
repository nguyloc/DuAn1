using UnityEngine;

namespace Script
{
    [System.Serializable]
    public class SlotClass
    {
        [SerializeField] ItemClass item;
        [SerializeField] int quantity = 0;


        public SlotClass()
        {
            item = null;
            quantity = 0;
        }

        public SlotClass(ItemClass _item, int _quanlity)
        {
            this.item = _item;
            this.quantity = _quanlity;
        }

        public ItemClass GetItem() { return this.item; }
        public int GetQuantity() { return this.quantity; }
        public void AddQuantity(int _quantity) { quantity += _quantity; }
        public void SubQuantity(int _quantity) { quantity -= _quantity; }


        public void AddItem(ItemClass item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }


        public void RemoveItem()
        {
            this.item = null;
            this.quantity = 0;
        }
    }
}