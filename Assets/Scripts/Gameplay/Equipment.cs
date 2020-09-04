using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] List<Card> gearCardsList = null;
    [SerializeField] ItemSlot[] gearSlots = null;

    [SerializeField] Inventory inventory = null;

    public int attackBonus;
    public int defenseBonus;

    [Space]
    [Header("Stats")]
    public int strengthBonus;
    public int intelligenceBonus;
    public int charmBonus;

    private void Awake()
    {
        gearCardsList = new List<Card>(gearSlots.Length);
        gameObject.SetActive(false);
    }

    public bool EquipItem(Card item)
    {
        foreach (ItemSlot gearSlot in gearSlots)
        {
            if (gearSlot.gearSlotType != item.itemType) continue;
            if (gearSlot.isBeingUsed)
            {
                DebugLogManager.instance.Log(gearSlot.name + " is already being used");
                return false;
            }
            else
            {
                gearSlot.AddItem(gearSlot.id, item.sprite,item.description);
                gearCardsList.Add(item);
                UpdateBonuses();
                return true;
            }
        }
        DebugLogManager.instance.Log("Couldn't find slot for the Item Type");
        return false;
    }

    public void UnequipItemTestingButton(int type)
    {
        Card.ItemType itemType=Card.ItemType.Head;
        switch (type)
        {
            case 0:
                itemType = Card.ItemType.Head;
                break;
            case 1:
                itemType = Card.ItemType.Chest;
                break;
            case 2:
                itemType = Card.ItemType.Legs;
                break;
            case 3:
                itemType = Card.ItemType.Accessory;
                break;
            case 4:
                itemType = Card.ItemType.HandLeft;
                break;
            case 5:
                itemType = Card.ItemType.HandRight;
                break;
        }
        UnequipItem(itemType);
    }

    public void UnequipItem(Card.ItemType item)
    {
        foreach (Card equiptCard in gearCardsList)
        {
            if (equiptCard.itemType != item) continue;
            if (inventory.AddItem(equiptCard))
            {
                foreach(ItemSlot gearSlot in gearSlots)
                {
                    if (gearSlot.gearSlotType != item) continue;
                    gearSlot.RemoveItem();
                    gearCardsList.Remove(equiptCard);
                    UpdateBonuses();
                    return;
                }
            }
        }
    }

    private void UpdateBonuses()
    {
        attackBonus = 0;
        defenseBonus = 0;

        strengthBonus = 0;
        intelligenceBonus = 0;
        charmBonus = 0;

        foreach (Card item in gearCardsList)
        {
            attackBonus += item.attack;
            defenseBonus += item.defense;

            strengthBonus += item.strength;
            intelligenceBonus += item.intelligence;
            charmBonus += item.charm;
        }
    }
}
