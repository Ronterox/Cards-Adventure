using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayCardInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Card card = null;

    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI descriptionText = null;
    [SerializeField] Image portrait = null;

    [SerializeField] TextMeshProUGUI hpText = null;
    [SerializeField] TextMeshProUGUI attackText = null;
    [SerializeField] TextMeshProUGUI defenseText = null;

    [SerializeField] Equipment equipment = null;

    [SerializeField] Image background = null;

    [SerializeField] Color enemyColor = Color.red;
    [SerializeField] Color npcColor = Color.cyan;
    [SerializeField] Color townColor = Color.white;
    [SerializeField] Color itemColor = Color.magenta;

    private Animator animator;

    private void Start()
    {
        UpdateCard();
        animator = GetComponent<Animator>();
    }

    public void UpdateCard()
    {
        if (card != null)
        {
            nameText.text = card.name;
            descriptionText.text = card.description;
            portrait.sprite = card.sprite;

            card.hp = card.maxHp;

            if (hpText != null)
                hpText.text = card.hp + "";
            if (attackText != null)
                attackText.text = card.attack + "";
            if (defenseText != null)
                defenseText.text = card.defense + "";
        }

        if (background != null)
        {
            switch (card.type)
            {
                case Card.CardType.Agressive:
                    background.color = enemyColor;
                    break;
                case Card.CardType.Environment:
                    background.color = townColor;
                    break;
                case Card.CardType.Item:
                    background.color = itemColor;
                    break;
                case Card.CardType.NPC:
                    background.color = npcColor;
                    break;
            }
        }
    }

    public void UpdateCardStats()
    {
        if (equipment != null)
        {
            if (hpText != null)
                hpText.text = card.hp + "";
            if (attackText != null)
                attackText.text = card.attack + equipment.attackBonus + "";
            if (defenseText != null)
                defenseText.text = card.defense + equipment.defenseBonus + "";
        }
        else
        {
            if (hpText != null)
                hpText.text = card.hp + "";
            if (attackText != null)
                attackText.text = card.attack + "";
            if (defenseText != null)
                defenseText.text = card.defense + "";
        }
    }

    private void OnValidate()
    {
        if (card != null)
        {
            nameText.text = card.name;
            descriptionText.text = card.description;
            portrait.sprite = card.sprite;

            if (hpText != null)
                hpText.text = card.hp + "";
            if (attackText != null)
                attackText.text = card.attack + "";
            if (defenseText != null)
                defenseText.text = card.defense + "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("flipped", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("flipped", false);
    }
}
