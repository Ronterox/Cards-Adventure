using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugLogManager : MonoBehaviour
{
    public static DebugLogManager instance = null;
    [SerializeField] GameObject descriptionText = null;

    private void Awake()
    {
        MakeSingleton();
    }
    private void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    public void Log(string text, float destructionTime = 1)
    {
        TextMeshProUGUI descText = Instantiate(descriptionText, FindObjectOfType<Canvas>().transform).GetComponent<TextMeshProUGUI>();

        descText.text = text;
        Destroy(descText.gameObject, destructionTime);
    }
}
