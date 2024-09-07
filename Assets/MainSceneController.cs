using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainSceneController : MonoBehaviour
{
    public Image record;
    public Sprite recordSprite, stopSprite;
    public TextMeshProUGUI topic;
    public TMP_Dropdown dropdownType;
    public TMP_Dropdown dropdownLanguage;
    bool isRecording = false;
    public RecognizeSpeech recognizeSpeech;
    // Start is called before the first frame update
    List<string> simulation;
    public List<string> simulationEn;
    public List<string> simulationSw;
    public List<string> simulationSp;
    void Start()
    {
        isRecording = false;

        print(PlayerPrefs.GetString("Lang", "En"));
        if (PlayerPrefs.GetString("Lang", "En") == "En")
        {
            dropdownType.value = 0;
            simulation = simulationEn;
        }
        else if (PlayerPrefs.GetString("Lang", "En") == "Sw")
        {
            dropdownType.value = 1;
            simulation = simulationSw;
        }
        else if (PlayerPrefs.GetString("Lang", "En") == "Sp")
        {
            dropdownType.value = 2;
            simulation = simulationSp;

        }
    }
    public void changeSimulation()
    {
        if (dropdownType.value == 0)
        {
            PlayerPrefs.SetString("Type", "Oral");

        }
        else if (dropdownType.value == 1)
        {
            PlayerPrefs.SetString("Type", "Interview");

        }

    }
    public void StartScene()
    {
        SceneManager.LoadScene(1);
        //TODO leave scene
    }
    public void changeLanguage()
    {
        if (dropdownLanguage.value == 0)
        {
            PlayerPrefs.SetString("Lang", "En");
            simulation = simulationEn;

        }
        else if (dropdownLanguage.value == 1)
        {
            PlayerPrefs.SetString("Lang", "Sw");
            simulation = simulationSw;

        }
        else if (dropdownLanguage.value == 2)
        {
            PlayerPrefs.SetString("Lang", "Sp");
            simulation = simulationSp;

        }
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        int indexofType = dropdownType.value ;
        dropdownType.options.Clear();

        foreach (string option in simulation)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option));
        }

        // Add the new options to the dropdown
        dropdownType.AddOptions(dropdownOptions);
        dropdownType.value=indexofType;
        updateText();
        recognizeSpeech.configure();

    }
    void updateText()
    {
        LanguageController[] langs= GetComponentsInChildren<LanguageController>();
        print(langs.Length);
        foreach(LanguageController l in langs)
        {
            l.Refresh();
        }
    }
    public void AddTopic()
    {
        print(isRecording);
        if (!isRecording)
        {
            dropdownType.enabled = false;
            dropdownLanguage.enabled = false;
            topic.text = "";
            record.sprite = stopSprite;
        }
        else
        {

            dropdownType.enabled = true;
            dropdownLanguage.enabled = true;
            record.sprite = recordSprite;
            PlayerPrefs.SetString("jobTitle", topic.text);

        }



        isRecording = !isRecording;
        recognizeSpeech.active = isRecording;




    }    
}
