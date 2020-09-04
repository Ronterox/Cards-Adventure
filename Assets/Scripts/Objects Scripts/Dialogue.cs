using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public float id;
    public string sentence;

    public string skill_check;
    public float fail_route;
    public int difficulty;

    public string prize;

    public string color;

    public bool isFinish;

    public string getEnding;

    /* Guide for creation of dialogues:
     * 1.- I suggest the use of yaml and then convert it to json because is faster to write.
     * 
     * 2.- The first item is the "name" of the speaker, item, enviroment. Example: 
     * "name":"character"
     * 
     * 3.- The second item is the array of "sentences" with each sentence and "sentence id" inside. Example:
     * "id": number
     * "sentence": this is what the speaker says
     * 
     * 4.- There is also the optional item "prize". Is used in manner of giving a type of prize to de player. Example:
     * "prize": armor
     * "prize": weapon
     * "prize": accessory
     * 
     * 5.- Another optional not so optional item is "skill_check" and "difficulty". They come together. They are used to let the player roll for success in the conversation. Example:
     * "skill_check": strength
     * "difficulty": 15
     * "fail_route": 0.5
     * 
     * "skill_check": intelligence
     * "difficulty": 10
     * "fail_route": 2.5
     * 
     * "skill_check": charm
     * "difficulty": 5
     * "fail_route": 5.5
     * 
     * note: fail_route is use to send the player to that conversation in case of skill_check failure.
     * 
     * note: difficulty goes from 5 to 30, with 5 being easy, 10 average, 15 tough, 20 hard, 25 really hard, 30 nearly impossible
     * 
     * 6.- "isFinish" bool set to true once you wanna complete the conversation.
     * 
     * About the "id":
     * 1.- The conversation id will increment 1 by 1. Example:
     *  "id": 1
     *  "sentence": Hello, my name is Pedro
     *  
     *  "id": 2
     *  "sentence": How are you today, are you okay?
     *  
     * 2.- Route Management:
     *  
     *  In what the id must end:
     *  In case of NPCs:
     *  #.1: Attack
     *  #.2: Ignore
     *  #.3: Persuade
     *  #.4: Flirt
     *  
     *  In case of Items:
     *  #.1: Pick Up
     *  #.2: Ignore
     *  #.3: Investigate
     *  #.4: Destroy
     *  
     *  In case of an Enviroment:
     *  #.1: Attack
     *  #.2: Ignore
     *  #.3: Investigate
     *  #.4: Rest
     *  
     */
}
