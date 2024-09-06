using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
[Serializable]
class openAiExpressionData
{
    public string prompt;
    public float temperature;
    public int max_tokens;
    public float top_p;
    public float frequency_penalty;
    public float presence_penalty;
    public string[] stop;
    public string user;
    public openAiExpressionData()
    {
        temperature = 0.5f;
        max_tokens = 60;
        top_p = 1;
        frequency_penalty = 0;
        presence_penalty = 0f;
        stop = new String[2];
        stop[0] = "Listener";
        stop[1] = "Speaker";
    }

}
[Serializable]
class openAiData
{
   
    public string model;
    public Message[] messages;
    public int max_tokens;
    public string user;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[Serializable]
public class choice
{
    public MessageResponse message;
    public int index;

}
[Serializable]
public class MessageResponse
{
    public string role;
    public string content;


}
[Serializable]
class openAiResponse
{
    public int index;
    public string created;
    public string model;
    public choice[] choices;


}


[Serializable]
class openAiExpressionResponse
{
    public string id;
    public string created;
    public string model;
    public choice[] choices;


}