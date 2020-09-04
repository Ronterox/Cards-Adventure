using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] TextMeshProUGUI dialogueBoxText = null;

    [SerializeField] GameObject endingPanel = null;

    [SerializeField] DialogueCollection[] dialogueCollections = null;

    [SerializeField] PrizeManager prizeManager = null;

    [SerializeField] Card playerCard = null;

    [SerializeField] Equipment playerEquipment = null;

    private int dialogueIndex = 0;
    public float sentencePointer = 0;

    private bool sentenceIsOver = true;
    private Coroutine typeCoroutine;

    private void Awake()
    {
        MakeSingleton();
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

    public void NextSentence()
    {
        StartCoroutine(NextSentenceEnumerator());
    }

    public IEnumerator NextSentenceEnumerator()
    {
        sentencePointer++;

        foreach (Dialogue sentence in dialogueCollections[dialogueIndex].sentences)
        {
            if (sentence.id == sentencePointer)
            {
                if (!string.IsNullOrEmpty(sentence.color))
                    ChangeDialogueTextColor(sentence.color);

                if (!string.IsNullOrEmpty(sentence.sentence))
                    TypeSentence(sentence.sentence);

                if (!string.IsNullOrEmpty(sentence.prize))
                    prizeManager.GivePrize(sentence.prize);

                //Game Manager Dependant
                if (!string.IsNullOrEmpty(sentence.getEnding))
                {
                    ShowEnding(sentence.getEnding);
                }
                else if (sentence.isFinish)
                {
                    yield return new WaitUntil(() => sentenceIsOver == true);
                    GameManager.instance.DisplayNextCard();
                }
                yield break;
            }
        }
        //Game Manager Dependant
        GameManager.instance.DisplayActionButtons(GameManager.instance.mainCardBase.card.type);
    }

    public void ChangeRoute(float newRoute)
    {
        sentencePointer = newRoute;

        ChangeDialogueTextColor("white");

        foreach (Dialogue sentence in dialogueCollections[dialogueIndex].sentences)
        {
            if (sentence.id == sentencePointer)
            {
                if (!string.IsNullOrEmpty(sentence.color))
                    ChangeDialogueTextColor(sentence.color);

                if (!string.IsNullOrEmpty(sentence.skill_check))
                {
                    if (RollSkillCheck(sentence.skill_check, sentence.difficulty))
                        TypeSentence(sentence.sentence);
                    else
                        ChangeRoute(sentence.fail_route);
                }
                else
                    TypeSentence(sentence.sentence);

                if (!string.IsNullOrEmpty(sentence.prize))
                    prizeManager.GivePrize(sentence.prize);
                return;
            }
        }
        Debug.LogError("Couldn't find sentence with id " + sentencePointer);
    }

    public void SetDialogue(string dialogueID)
    {
        int counter = 0;
        foreach (DialogueCollection collection in dialogueCollections)
        {
            if (collection.name == dialogueID)
            {
                dialogueIndex = counter;
                sentencePointer = 0;

                dialogueBoxText.color = Color.white;

                LoadSentences();
                NextSentence();
                return;
            }
            counter++;
        }
        Debug.LogError("Couldn't find dialogue with the dialogueID " + dialogueID);
    }

    private void LoadSentences()
    {
        if (dialogueCollections[dialogueIndex].sentences.Length == 0)
        {
            dialogueCollections[dialogueIndex] = JsonUtility.FromJson<DialogueCollection>(dialogueCollections[dialogueIndex].dialogueAsset.text);

            Debug.Log("Load completed");
        }
        else
            Debug.Log("Already loaded");
    }

    private bool RollSkillCheck(string skill, int difficulty)
    {
        int roll = 1;
        switch (skill)
        {
            case "strength":
                roll = Random.Range(0, 21);
                if (playerCard != null)
                    roll += playerCard.strength;
                if (playerEquipment != null)
                    roll += playerEquipment.strengthBonus;
                break;
            case "intelligence":
                roll = Random.Range(0, 21);
                if (playerCard != null)
                    roll += playerCard.intelligence;
                if (playerEquipment != null)
                    roll += playerEquipment.attackBonus;
                break;
            case "charm":
                roll = Random.Range(0, 21);
                if (playerCard != null)
                    roll += playerCard.charm;
                if (playerEquipment != null)
                    roll += playerEquipment.charmBonus;
                break;
        }

        if (roll >= difficulty)
        {
            AudioManager.instance.Play("roll success");
            DebugLogManager.instance.Log("Roll Successful: " + roll + " out of " + difficulty, 3);
            return true;
        }
        AudioManager.instance.Play("roll unsuccess");
        DebugLogManager.instance.Log("Roll Unsuccessful: " + roll + " out of " + difficulty, 3);
        return false;
    }

    public void ShowEnding(string endingDialogue)
    {
        SetDialogue(endingDialogue);
        endingPanel.SetActive(true);
        dialogueBoxText = endingPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private IEnumerator SayInDialogBox(string sentence, float speed)
    {
        sentenceIsOver = false;
        dialogueBoxText.text = "";
        foreach (char character in sentence.ToCharArray())
        {
            dialogueBoxText.text += character;
            AudioManager.instance.Play("type");
            yield return new WaitForSeconds(speed);
        }
        sentenceIsOver = true;
    }

    private void TypeSentence(string sentence, float speed = 0.040f)
    {
        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);
        typeCoroutine = StartCoroutine(SayInDialogBox(sentence, speed));
    }

    private void ChangeDialogueTextColor(string color)
    {
        switch (color)
        {
            case "white":
                dialogueBoxText.color = Color.white;
                break;
            case "cyan":
                dialogueBoxText.color = Color.cyan;
                break;
            default:
                Debug.LogError("Color " + color + " doesn't exist in the script.");
                break;
        }
    }
}
