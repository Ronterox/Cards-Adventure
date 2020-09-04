using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    [TextArea(3, 12)]
    public string description;
    public Sprite sprite;

    [Space]
    public int level = 1;

    [HideInInspector]
    public int hp;
    [Space]
    public int maxHp;
    public int attack;
    public int defense;

    [Space]
    [Header("Stats")]
    public int strength;
    public int intelligence;
    public int charm;

    public CardType type;

    [HideInInspector]
    public ItemType itemType;

    public enum CardType
    {
        NPC,
        Item,
        Environment,
        Agressive,
        PlayerCharacter
    }

    public enum ItemType
    {
        Head,
        Chest,
        Legs,
        HandRight,
        HandLeft,
        Accessory
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Card)), CanEditMultipleObjects]
public class CardEditor : Editor
{
    override public void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        var myScript = target as Card;

        if (myScript.type.Equals(Card.CardType.Item))
            myScript.itemType = (Card.ItemType)EditorGUILayout.EnumPopup("Item Type", myScript.itemType);

    }
}
#endif