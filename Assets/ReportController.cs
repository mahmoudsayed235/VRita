using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using TMPro;
public class ReportController : MonoBehaviour
{

    public string exam;
    List<Message> messages = new List<Message>();
    public GeneratedText generatedText;
    public string prompt;
    public string promptEngInterview;
    public string promptSwiInterview;
    public string promptSpInterview;

    public string promptEngOral;
    public string promptSwiOral;
    public string promptSpOral;
    string openAIDomain;
    private void Awake()
    {
        
            if (PlayerPrefs.GetString("Lang", "En") == "En")
            {
                prompt = promptEngOral;
            }
            else if (PlayerPrefs.GetString("Lang", "En") == "Sw")
            {
                prompt = promptSwiOral;
            }
            else if (PlayerPrefs.GetString("Lang", "En") == "Sp")
            {
                prompt = promptSpOral;
            }
        
        
        openAIDomain = "https://api.openai.com/v1/chat/completions";
    }
    public void GenerateReportFunction()
    {
        GenerateReport();
    }
    public async Task GenerateReport()
    {
        await GetText(true, "");
    }
    public InteractionManager interactionManager;
    public RecognizeSpeech recognizeSpeech;
    public GameObject loader;
    public async Task GetText(bool generate, string base_response)
    {
        loader.SetActive(true);
        synthesizeSpeech.StopAudio();
        interactionManager.enabled=false;
        recognizeSpeech.gameObject.SetActive(false);
        exam = generatedText.prompt;
        string response = "";
      
        string pattern = @"\b_exam\b";
        //for progression
        //  prompt = prompts[index];
        prompt = Regex.Replace(prompt, pattern, exam);
        

        if (generate)
        {
            openAiData openAIdata = new openAiData();
            openAIdata.model = "gpt-4o";

            Message m = new Message { role = "system", content =  prompt };
            messages.Add(m);
            

            openAIdata.messages = messages.ToArray();
            openAIdata.max_tokens = 150;
            openAIdata.user = "12AA24124";
            string json = JsonUtility.ToJson(openAIdata);
            //https://api.openai.com/v1/engines/gpt-4/completions for English
            using (UnityWebRequest www = UnityWebRequest.Put(openAIDomain, json))
            {
                www.method = "POST";
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("Authorization", "Bearer " + LoadKeys.OPEN_AI_API_KEY.ToString());

                var operation = www.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }
                if (www.result != UnityWebRequest.Result.Success)
                {
                    print(www.downloadHandler.text);
                    Debug.Log(www.error);
                }
                else
                {
                    print(www.downloadHandler.text);
                    openAiResponse openAIresponse = JsonUtility.FromJson<openAiResponse>(www.downloadHandler.text);

                    if (openAIresponse.choices.Length > 0)
                    {

                       response = openAIresponse.choices[0].message.content;
                        data=JsonUtility.FromJson<Data>(response);
                        FetchReport(data);
                    }
                }

            }
            
        }
        else
        {
           response = base_response;

        }

       
    }
    Data data;
    public SynthesizeSpeech synthesizeSpeech;
    public void PlaySummary()
    {
        synthesizeSpeech.SynthesizeAudioAsync(data.summary,false,"");
    }
    public GameObject reportObj;
    public TextMeshProUGUI[] section;
    public Image[] image;
    public TextMeshProUGUI[] sectionScore;

    void FetchReport(Data data)
    {
    
        for(int i = 0; i < data.data.Count; i++)
        {
            section[i].text = data.data[i].section;
            sectionScore[i].text = data.data[i].score.ToString();
            image[i].fillAmount = (float)data.data[i].score/100.0f;
        }
        reportObj.SetActive(true);
    }
    
}
[System.Serializable]
public class Data
{
    public List<Section> data;
    public string summary;
}
[System.Serializable]
public class Section
{
    public string section;
    public int score;
}