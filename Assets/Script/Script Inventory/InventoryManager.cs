using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject SetInventory;
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

        SetInventory.SetActive(false);

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

        if (Input.GetMouseButtonDown(1))
        {
            if (isMoving)
            {
                //EndMove();
            }
            else
            {
                BeginSplit();
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

        ToggleInventory();
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

        if (originalSlot == null || originalSlot.GetItem() == null) return;

        movingSlot.AddItem(
        originalSlot.GetItem(),
        originalSlot.GetQuantity());
        originalSlot.RemoveItem();

        isMoving = true;
        RefreshUI();
        return;
    }

    private void BeginSplit()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null) return;

        if (originalSlot.GetQuantity() <= 1) return;

        movingSlot.AddItem(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));

        originalSlot.SubQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));

        isMoving = true;
        RefreshUI();
        return;
    }
    private void EndMove()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                //if slot is the same item
                if (originalSlot.GetItem() == movingSlot.GetItem())
                {
                    //if slot item is stackable
                    if (originalSlot.GetItem().isStackable)
                    {
                        int ItemMaxStack = originalSlot.GetItem().StackQuantity;
                        int count = originalSlot.GetQuantity() + movingSlot.GetQuantity();

                        if (count > ItemMaxStack)
                        {
                            int remain = count - ItemMaxStack;
                            originalSlot.SetQuantity(ItemMaxStack);
                            movingSlot.SetQuantity(remain);

                            isMoving = true;
                            RefreshUI();
                            return;
                        }
                        else
                        {
                            originalSlot.AddQuantity(originalSlot.GetQuantity());
                            movingSlot.RemoveItem();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    //swap
                    tempSlot.AddItem(originalSlot.GetItem(), originalSlot.GetQuantity());
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    tempSlot.RemoveItem();

                    RefreshUI();
                    return;
                }
            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.RemoveItem();
            }
        }


        isMoving = false;
        RefreshUI();
        return;
    }

    private void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetInventory.SetActive(!SetInventory.activeSelf);
        }
        
    }


}
