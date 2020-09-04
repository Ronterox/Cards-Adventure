using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Card> itemCardsList = null;
    [SerializeField] ItemSlot[] itemSlots = null;

    [SerializeField] Equipment equipment = null;

    [SerializeField] Animator buttonAnimator = null;

    private void Awake()
    {
        itemCardsList = new List<Card>(itemSlots.Length);
        gameObject.SetActive(false);
    }

    public bool AddItem(Card item)
    {
        if (item.type != Card.CardType.Item)
        {
            Debug.LogError("Tried to add an not item Card");
            return false;
        }

        if (itemCardsList.Count.Equals(itemCardsList.Capacity))
        {
            DebugLogManager.instance.Log("Inventory is full");
            return false;
        }
        itemCardsList.Add(item);
        RefreshInventory();

        if (buttonAnimator != null)
            buttonAnimator.SetBool("highlight", true);

        return true;
    }

    public void RefreshInventory()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.RemoveItem();
        }

        int index = 0;
        foreach (Card item in itemCardsList)
        {
            foreach (ItemSlot slot in itemSlots)
            {
                if (!slot.isBeingUsed)
                {
                    slot.AddItem(index, item.sprite, item.description);
                    index++;
                    break;
                }
            }
        }
    }

    public void RemoveItem(int itemID)
    {
        if (itemCardsList.Count == 0)
            return;

        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.id != itemID) continue;
            slot.RemoveItem();
            itemCardsList.RemoveAt(itemID);
            RefreshInventory();
            return;
        }
    }

    public void EquipItem(int itemID)
    {
        if (itemCardsList.Count == 0 || equipment == null)
            return;

        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.id != itemID) continue;
            if (equipment.EquipItem(itemCardsList[itemID]))
                RemoveItem(itemID);
        }

        AudioManager.instance.Play("equip");
    }
}
