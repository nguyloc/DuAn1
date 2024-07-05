using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject slotHolder;
        [SerializeField] private ItemClass itemToAdd;
        [SerializeField] private ItemClass itemToRemove;
        [SerializeField] private SlotClass[] items;
        [SerializeField] private SlotClass[] startingItems;

        [SerializeField] private SlotClass movingSlot;
        [SerializeField] private SlotClass originalSlot;
        [SerializeField] private SlotClass tempSlot;

        public Image itemCursor;

        [SerializeField] private GameObject[] slots;
        public bool isMoving;

        // Start is called before the first frame update
        void Start()
        {
            slots = new GameObject[slotHolder.transform.childCount];
            items = new SlotClass[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = slotHolder.transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new SlotClass();
            }

            for (int i = 0; i < startingItems.Length; i++)
            {
                items[i] = startingItems[i];
            }

            originalSlot = new SlotClass();
            movingSlot = new SlotClass();
            tempSlot = new SlotClass();

            RefreshUI();   
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isMoving)
                {
                    EndMove();
                }
                else
                {
                    BeginMove();
                }
            }

            if (isMoving)
            {
                itemCursor.enabled = true;
                itemCursor.transform.position = Input.mousePosition;
                itemCursor.sprite = movingSlot.GetItem().itemIcon;
            }
            else
            {
                itemCursor.enabled = false;
                itemCursor.sprite = null;
            }
        }

        private void RefreshUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                try
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                    if (!items[i].GetItem().isStackable)
                    {
                        slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    }
                    else
                    {
                        slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity() + "";
                    }

                }
                catch
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }

        public void AddItem(ItemClass item, int quantity)
        {
            SlotClass slot = ContainsItem(item);
            if (slot != null && slot.GetItem().isStackable)
            {
                slot.AddQuantity(quantity);
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == null)
                    {
                        items[i].AddItem(item, quantity);
                        break;
                    }
                }
            }

        }

        public void RemoveItem(ItemClass item, int quantity)
        {
            SlotClass temp = ContainsItem(item);
            if (temp != null)
            {
                if (temp.GetQuantity() > 1)
                {
                    temp.SubQuantity(quantity);
                }
                else
                {
                    int slotToRemoveIndex = 0;
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].GetItem() == item)
                        {
                            slotToRemoveIndex = i;
                            break;
                        }
                    }

                    items[slotToRemoveIndex].RemoveItem();
                }
            }
            else
            {
                return;
            }



            RefreshUI();
        }

        private SlotClass ContainsItem(ItemClass item)
        {

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == item)
                {
                    return items[i];
                }
            }
            return null;
        }
        private SlotClass GetClosestSlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 64)
                {
                    return items[i];
                }
            }

            return null;
        }

        private void BeginMove()
        {
            originalSlot = GetClosestSlot();

            if (originalSlot.GetItem() == null) return;

            movingSlot.AddItem(
                               originalSlot.GetItem(),
                               originalSlot.GetQuantity());
            originalSlot.RemoveItem();

            isMoving = true;
            RefreshUI();
            return;
        }
        private void EndMove()
        {
            originalSlot = GetClosestSlot();

            if(originalSlot.GetItem() != null)
            {
                if(originalSlot.GetItem() == movingSlot.GetItem())
                {
                    if(originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(originalSlot.GetQuantity());
                        originalSlot.RemoveItem();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    //swap
                    tempSlot.AddItem(originalSlot.GetItem(),originalSlot.GetQuantity());
                    originalSlot.AddItem(movingSlot.GetItem(),movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(),tempSlot.GetQuantity());

                    RefreshUI();
                    return;
                }
            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.RemoveItem();
            }
            isMoving = false;
            RefreshUI();
       
        }
    }
}
