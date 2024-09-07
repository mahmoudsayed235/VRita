using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;






public class GeneratedText : MonoBehaviour
{
    [HideInInspector]
    public string response = "";
    //[HideInInspector]
    public string prompt;

    public string promptEngInterview;
    public string promptSwiInterview;
    public string promptSpInterview;

    public string promptEngOral;
    public string promptSwiOral;
    public string promptSpOral;
    string previousPrompt = "";
    public string jobDescription = "";
    string[] talker;
    public string[] talkerEng;
    string openAIDomain = "";
    int index = 0;
    private void Awake()
    {



        openAIDomain = "https://api.openai.com/v1/chat/completions";
        talker = talkerEng;
        //PlayerPrefs.SetString("Type", "Interview");
        if (PlayerPrefs.GetString("Type", "Oral") == "Oral")
        {
            print("Oral");
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
        }else if (PlayerPrefs.GetString("Type", "Oral") == "Interview")
        {
            if (PlayerPrefs.GetString("Lang", "En") == "En")
            {
                prompt = promptEngInterview;
            }
            else if (PlayerPrefs.GetString("Lang", "En") == "Sw")
            {
                prompt = promptSwiInterview;
            }
            else if (PlayerPrefs.GetString("Lang", "En") == "Sp")
            {
                prompt = promptSpInterview;
            }
        }

        updatePrompt();

    }
    private void Start()
    {
    }
    void Update()
    {
    }
    public void updatePrompt()
    {
        response = "";
        string pattern = @"\b_jobDescrition\b";
        //for progression
        //  prompt = prompts[index];
        if (PlayerPrefs.GetString("jobTitle") != "")
        {
            prompt = Regex.Replace(prompt, pattern, PlayerPrefs.GetString("jobTitle"));
        }
        else
        {
            prompt = Regex.Replace(prompt, pattern, jobDescription);

        }
    }
    List<Message> messages = new List<Message>();
    public async Task GetText(string playerInput, bool generate, string base_response)
    {
        print("start generated : " + Time.time);
        string promptInput = "";


        promptInput = prompt + $"\n\n user: {playerInput} \n system: ";
        print($"prompt input: {promptInput}");

        if (generate)
        {
            openAiData openAIdata = new openAiData();
            openAIdata.model = "gpt-4o"; 

            Message m = new Message { role = "system", content = prompt };
            messages.Add(m);
            m = new Message { role = "user", content = $" {playerInput}" };
            messages.Add(m);
            m = new Message { role = "system", content = $"" };
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

                        
                        promptInput += $"{openAIresponse.choices[0].message.content} \n";
                        response = openAIresponse.choices[0].message.content;
                        prompt = promptInput;
                        print(response);
                       

                    }
                }

            }
            
        }
        else
        {
            response = base_response;

        }


    }

    public string GetResponse()
    {
        return response;
    }

    public string GetPrompt()
    {

        return prompt;
    }

    void Disable()
    {
        response = "";

    }

}