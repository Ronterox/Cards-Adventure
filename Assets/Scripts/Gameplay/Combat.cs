using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combat : MonoBehaviour
{
    public Card playerCard = null;
    public Card enemyCard = null;
    [SerializeField] int maxHit = 20;

    [SerializeField] Equipment playerEquipment = null;

    [SerializeField] Animator playerAnimator = null;
    [SerializeField] Animator enemyAnimator = null;

    [SerializeField] DisplayCardInfo playerCardDisplay = null;
    [SerializeField] DisplayCardInfo enemyCardDisplay = null;

    [SerializeField] GameObject hitEffect = null;
    [SerializeField] GameObject damageTextObj = null;

    [SerializeField] float eachTurnSeconds = 3;

    private bool finished = false;

    //CODIGO ASQUEROSO EL DE FIGHT(), pero funciona. Arreglarlo al trabajar en serio.
    public void StartFight()
    {
        StartCoroutine(Fight());
        AudioManager.instance.Stop("adventure");
        AudioManager.instance.Play("combat");
    }
    private IEnumerator Fight()
    {
        while (!finished)
        {
            if (enemyCard.type.Equals(Card.CardType.Agressive))
            {
                //Enemy Turn
                if (enemyCard.hp < 1)
                {
                    DialogueManager.instance.ChangeRoute(999);
                    finished = true;
                    break;
                }
                else if (AttackPlayer())
                {
                    enemyAnimator.SetTrigger("attack");
                    AudioManager.instance.Play("hit");

                    if (playerCardDisplay != null)
                        playerCardDisplay.UpdateCardStats();
                    if (hitEffect != null)
                        Destroy(Instantiate(hitEffect, playerCardDisplay.gameObject.transform), 0.1f);
                }
                else
                {
                    enemyAnimator.SetTrigger("fail");
                    AudioManager.instance.Play("miss");
                }

                //Player Turn
                if (playerCard.hp < 1)
                {
                    DialogueManager.instance.ShowEnding("Player Death");
                    finished = true;
                    break;
                }
                if (AttackEnemy())
                {
                    playerAnimator.SetTrigger("attack");
                    AudioManager.instance.Play("hit");

                    if (enemyCardDisplay != null)
                        enemyCardDisplay.UpdateCardStats();
                    if (hitEffect != null)
                        Destroy(Instantiate(hitEffect, enemyCardDisplay.gameObject.transform), 0.1f);
                }
                else
                {
                    playerAnimator.SetTrigger("fail");
                    AudioManager.instance.Play("miss");
                }
                yield return new WaitForSeconds(eachTurnSeconds);
            }
            else
            {
                //Player turn
                if (playerCard.hp < 1)
                {
                    DialogueManager.instance.ShowEnding("Player Death");
                    finished = true;
                    break;
                }
                else if (AttackEnemy())
                {
                    playerAnimator.SetTrigger("attack");
                    AudioManager.instance.Play("hit");

                    if (enemyCardDisplay != null)
                        enemyCardDisplay.UpdateCardStats();
                    if (hitEffect != null)
                        Destroy(Instantiate(hitEffect, enemyCardDisplay.gameObject.transform), 0.1f);
                }
                else
                {
                    playerAnimator.SetTrigger("fail");
                    AudioManager.instance.Play("miss");
                }

                //Enemy Turn
                if (enemyCard.hp < 1)
                {
                    DialogueManager.instance.ChangeRoute(999);
                    finished = true;
                    break;
                }
                else if (AttackPlayer())
                {
                    enemyAnimator.SetTrigger("attack");
                    AudioManager.instance.Play("hit");

                    if (playerCardDisplay != null)
                        playerCardDisplay.UpdateCardStats();
                    if (hitEffect != null)
                        Destroy(Instantiate(hitEffect, playerCardDisplay.gameObject.transform), 0.1f);
                }
                else
                {
                    enemyAnimator.SetTrigger("fail");
                    AudioManager.instance.Play("miss");
                }
                yield return new WaitForSeconds(eachTurnSeconds);
            }
            DialogueManager.instance.NextSentence();
        }
        finished = false;
        AudioManager.instance.Stop("combat");
        if (playerCard.hp > 0)
            AudioManager.instance.Play("adventure");
    }

    private bool AttackEnemy()
    {
        int hit = Random.Range(1, maxHit + 1);
        Debug.Log(playerCard.name + " roll " + hit);

        if (hit >= enemyCard.defense)
        {
            int damage = 0;
            if (hit == maxHit)
            {
                damage = playerCard.attack * 2 + playerEquipment.attackBonus;
                enemyCard.hp -= damage;
                Debug.Log(enemyCard.name + " received " + damage);

                if (damageTextObj != null && enemyCardDisplay != null)
                {
                    GameObject instance = Instantiate(damageTextObj, enemyCardDisplay.transform);
                    instance.GetComponent<TextMeshProUGUI>().text = "-" + damage + " CRITICAL!";
                    Destroy(instance, 0.3f);
                }
            }
            else
            {
                damage = playerCard.attack + playerEquipment.attackBonus;
                enemyCard.hp -= damage;
                Debug.Log(enemyCard.name + " received " + damage);

                if (damageTextObj != null && enemyCardDisplay != null)
                {
                    GameObject instance = Instantiate(damageTextObj, enemyCardDisplay.transform);
                    instance.GetComponent<TextMeshProUGUI>().text = "-" + damage;
                    Destroy(instance, 0.3f);
                }
            }
            return true;
        }
        return false;
    }

    private bool AttackPlayer()
    {
        int hit = Random.Range(1, maxHit + 1);
        Debug.Log(enemyCard.name + " rolled " + hit);
        hit += playerCard.strength;
        if (playerEquipment != null)
            hit += playerEquipment.strengthBonus;

        int playerDefense = playerCard.defense;
        if (playerEquipment != null)
            playerDefense += playerEquipment.defenseBonus;

        if (hit >= playerDefense)
        {
            int damage = 0;
            if (hit == maxHit)
            {
                damage = enemyCard.attack * 2;
                playerCard.hp -= damage;
                Debug.Log(playerCard.name + " received " + damage);

                if (damageTextObj != null && playerCardDisplay != null)
                {
                    GameObject instance = Instantiate(damageTextObj, playerCardDisplay.transform);
                    instance.GetComponent<TextMeshProUGUI>().text = "-" + damage + " CRITICAL!";
                    Destroy(instance, 0.3f);
                }
            }
            else
            {
                damage = enemyCard.attack;
                playerCard.hp -= damage;
                Debug.Log(playerCard.name + " received " + damage);

                if (damageTextObj != null && playerCardDisplay != null)
                {
                    GameObject instance = Instantiate(damageTextObj, playerCardDisplay.transform);
                    instance.GetComponent<TextMeshProUGUI>().text = "-" + damage;
                    Destroy(instance, 0.3f);
                }

            }
            return true;
        }
        return false;
    }

    public void TryEscape()
    {
        AudioManager.instance.Play("dice");
        int roll = Random.Range(0, 21);
        if (roll >= 10)
        {
            finished = true;
            DebugLogManager.instance.Log("Escaped completed");
            GameManager.instance.DisplayNextCard();
        }
        else
            DebugLogManager.instance.Log("Failed to escape");
    }

    public void HealPlayer(int heal = 1000)
    {
        playerCard.hp += heal;
        if (playerCard.hp > playerCard.maxHp)
            playerCard.hp = playerCard.maxHp;
        if (playerCardDisplay != null)
            playerCardDisplay.UpdateCardStats();
    }
}
