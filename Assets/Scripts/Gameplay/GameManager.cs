using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public DisplayCardInfo mainCardBase = null;

    [Space]
    [Header("Cards")]
    [SerializeField] Card[] npcs = null;

    [SerializeField] Card[] equipCards = null;

    [SerializeField] Card[] enviroments = null;

    [SerializeField] Card[] miniBosses = null;

    [SerializeField] Card[] lastBosses = null;

    [SerializeField] GameObject[] actionButtons = null;

    [SerializeField] Combat combat = null;

    [SerializeField] Inventory inventory = null;

    private Animator mainCardAnimator;
    private int cardCounter = 0;
    private List<Card> lastCards = new List<Card>();

    //Temporal index
    private int index = 0;

    private void Awake()
    {
        mainCardAnimator = mainCardBase.gameObject.GetComponent<Animator>();
        MakeSingleton();
    }
    private void Start()
    {
        AudioManager.instance.Play("adventure");
        DisplayNextCard();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            DialogueManager.instance.NextSentence();
    }

    private void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
    }

    public void DisplayNextCard()
    {
        if (mainCardBase == null)
        {
            Debug.LogError("GameManager is missing components");
            return;
        }

        cardCounter++;

        mainCardBase.card = getNextCard();

        mainCardBase.UpdateCard();

        lastCards.Add(mainCardBase.card);

        combat.enemyCard = mainCardBase.card;
        if (combat.enemyCard.type.Equals(Card.CardType.Agressive))
            combat.StartFight();

        DisplayActionButtons(mainCardBase.card.type, true);

        DialogueManager.instance.SetDialogue(mainCardBase.card.name);

        AudioManager.instance.Play("swap_card");

        mainCardAnimator.SetTrigger("swap");
    }

    public void DisplayActionButtons(Card.CardType cardType, bool newCard = false)
    {
        foreach (GameObject button in actionButtons)
        {
            if (button.name.Equals(cardType.ToString()))
            {
                button.SetActive(true);
                if (newCard)
                {
                    foreach (Transform child in button.GetComponentInChildren<Transform>())
                    {
                        if (cardType.Equals(Card.CardType.NPC) && child.name.Equals("Button Run"))
                            child.gameObject.SetActive(false);
                        else
                            child.gameObject.SetActive(true);
                    }
                }
            }
            else
                button.SetActive(false);
        }
    }

    public void AddItemToInventory()
    {
        inventory.AddItem(mainCardBase.card);
    }

    private bool CardWasUsed(Card nextCard)
    {
        if (lastCards.Count == 0)
            return false;

        int counter = 0;
        foreach (Card card in lastCards)
        {
            if (card == nextCard) return true;
            counter++;
        }

        switch (nextCard.type)
        {
            //if all cards of an array are used
            case Card.CardType.NPC:
                if (counter == npcs.Length)
                    return false;
                break;
            case Card.CardType.Item:
                if (counter == equipCards.Length)
                    return false;
                break;
        }

        return false;
    }

    private Card getNextCard()
    {
        Card nextCard;
        int random;
        if (cardCounter % 5 == 0)
        {
            if (cardCounter % 10 == 0)
            {
                random = Random.Range(0, lastBosses.Length);
                nextCard = lastBosses[random];
            }
            else
            {
                random = Random.Range(0, miniBosses.Length);
                nextCard = miniBosses[random];
            }
        }
        else if ((cardCounter + 1) % 5 == 0)
        {
            nextCard = enviroments[index];
            index++;
        }
        else
        {
            random = Random.Range(0, 21);
            if (random <= 15)
            {
                do
                {
                    random = Random.Range(0, npcs.Length);
                    nextCard = npcs[random];
                }
                while (CardWasUsed(nextCard));
            }
            else
            {
                do
                {
                    random = Random.Range(0, equipCards.Length);
                    nextCard = equipCards[random];
                }
                while (CardWasUsed(nextCard));
            }
        }

        return nextCard;
    }
}
