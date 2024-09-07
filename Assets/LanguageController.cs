using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageController : MonoBehaviour
{
    public string En, Sw, Sp;
    // Start is called before the first frame update
    void OnEnable()
    {
        UpdateText();
    }
    private void UpdateText()
    {
        print(PlayerPrefs.GetString("Lang"));
        if (PlayerPrefs.GetString("Lang", "En") == "En")
        {
            GetComponent<TextMeshProUGUI>().text = En;
        }
        else if (PlayerPrefs.GetString("Lang", "En") == "Sw")
        {
            GetComponent<TextMeshProUGUI>().text = Sw;
        }
        else if (PlayerPrefs.GetString("Lang", "En") == "Sp")
        {
            GetComponent<TextMeshProUGUI>().text = Sp;
        }
    }
    public void Refresh()
    {
        UpdateText();
    }
}
