using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    [SerializeField] Image itemImage = null;
    [SerializeField] TextMeshProUGUI descText = null;
    [SerializeField] Sprite defaultSprite = null;
    [HideInInspector]
    public bool isBeingUsed = false;

    public bool isGearSlot;

    [HideInInspector]
    public Card.ItemType gearSlotType;

    public string description = "Inventory Space for an Item";
    [SerializeField]
    private string defaultDescription = "Inventory Space for an Item";

    private string oldText;
    private void Awake()
    {
        description = defaultDescription;
    }
    public void AddItem(int itemID, Sprite itemSprite, string desc)
    {
        id = itemID;
        itemImage.sprite = itemSprite;
        isBeingUsed = true;

        description = desc;
    }

    public void RemoveItem()
    {
        itemImage.sprite = defaultSprite;
        isBeingUsed = false;

        description = defaultDescription;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        oldText = descText.text;
        descText.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descText.text = oldText;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ItemSlot)), CanEditMultipleObjects]
public class ItemSlotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var myScript = target as ItemSlot;

        if (myScript.isGearSlot)
        {
            myScript.gearSlotType = (Card.ItemType)EditorGUILayout.EnumPopup("Gear Slot Type", myScript.gearSlotType);
            EditorUtility.SetDirty(myScript);
        }
    }
}
#endif
