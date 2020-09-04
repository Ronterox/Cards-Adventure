using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeManager : MonoBehaviour
{
    [SerializeField] Card[] weapon = null;
    [SerializeField] Card[] weaponQuality = null;
    [SerializeField] Card[] weaponRare = null;
    [SerializeField] Card[] weaponLegendary = null;

    [SerializeField] Card[] accessory = null;
    [SerializeField] Card[] accessoryQuality = null;
    [SerializeField] Card[] accessoryRare = null;
    [SerializeField] Card[] accessoryLegendary = null;

    [SerializeField] Card[] armor = null;
    [SerializeField] Card[] armorQuality = null;
    [SerializeField] Card[] armorRare = null;
    [SerializeField] Card[] armorLegendary = null;

    [SerializeField] Inventory inventory = null;

    [SerializeField] DisplayCardInfo playerCardDisplay = null;

    [SerializeField] float porcentageDamage = 10;

    [SerializeField] GameObject hitEffect = null;

    public void GivePrize(string prize)
    {
        if (weapon.Length == 0 || accessory.Length == 0 || armor.Length == 0)
        {
            Debug.LogError("Not enough prizes");
            return;
        }

    Reset:
        switch (prize)
        {
            case "armor":
                int random = Random.Range(0, armor.Length);
                inventory.AddItem(armor[random]);
                break;
            case "armor quality":
                random = Random.Range(0, armorQuality.Length);
                inventory.AddItem(armorQuality[random]);
                break;
            case "armor rare":
                random = Random.Range(0, armorRare.Length);
                inventory.AddItem(armorRare[random]);
                break;
            case "armor legendary":
                random = Random.Range(0, armorLegendary.Length);
                inventory.AddItem(armorLegendary[random]);
                break;

            case "weapon":
                random = Random.Range(0, weapon.Length);
                inventory.AddItem(weapon[random]);
                break;
            case "weapon quality":
                random = Random.Range(0, weaponQuality.Length);
                inventory.AddItem(weaponQuality[random]);
                break;
            case "weapon rare":
                random = Random.Range(0, weaponRare.Length);
                inventory.AddItem(weaponRare[random]);
                break;
            case "weapon legendary":
                random = Random.Range(0, weaponLegendary.Length);
                inventory.AddItem(weaponLegendary[random]);
                break;

            case "accessory":
                random = Random.Range(0, accessory.Length);
                inventory.AddItem(accessory[random]);
                break;
            case "accessory quality":
                random = Random.Range(0, accessoryQuality.Length);
                inventory.AddItem(accessoryQuality[random]);
                break;
            case "accessory rare":
                random = Random.Range(0, accessoryRare.Length);
                inventory.AddItem(accessoryRare[random]);
                break;
            case "accessory legendary":
                random = Random.Range(0, accessoryLegendary.Length);
                inventory.AddItem(accessoryLegendary[random]);
                break;

            case "all":
                random = Random.Range(0, armor.Length);
                inventory.AddItem(armor[random]);
                random = Random.Range(0, weapon.Length);
                inventory.AddItem(weapon[random]);
                random = Random.Range(0, accessory.Length);
                inventory.AddItem(accessory[random]);
                break;

            case "random":
                random = Random.Range(0, 11);
                switch (random)
                {
                    case 0:
                        prize = "armor";
                        break;
                    case 1:
                        prize = "weapon";
                        break;
                    case 2:
                        prize = "armor quality";
                        break;
                    case 3:
                        prize = "weapon quality";
                        break;
                    case 4:
                        prize = "accessory quality";
                        break;
                    case 5:
                        prize = "armor rare";
                        break;
                    case 6:
                        prize = "weapon rare";
                        break;
                    case 7:
                        prize = "accessory rare";
                        break;
                    case 8:
                        prize = "armor legendary";
                        break;
                    case 9:
                        prize = "weapon legendary";
                        break;
                    case 10:
                        prize = "accessory legendary";
                        break;
                }
                goto Reset;
            case "damage":
                playerCardDisplay.card.hp -= (int)porcentageDamage * playerCardDisplay.card.maxHp / 100;
                playerCardDisplay.UpdateCardStats();
                AudioManager.instance.Play("hit");
                if (hitEffect != null)
                    Destroy(Instantiate(hitEffect, playerCardDisplay.gameObject.transform), 0.1f);

                if (playerCardDisplay.card.hp < 1)
                    DialogueManager.instance.ShowEnding("player death");
                break;

            default:
                Debug.LogError("There is no such prize");
                break;
        }
    }
}
