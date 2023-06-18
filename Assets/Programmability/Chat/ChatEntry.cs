using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatEntry : ChatMenuElement
{
    public Chat chat;
    /*public TextMesh senderName;
    public TextMesh message;*/
    public Action Clicked => GetComponentInParent<Blobchat>().OpenChat(/*chat*/);
    public string chatName;
    //private const int maxMessageLength = 50;

    public void Initialize(Chat chat)
    {
        this.chat = chat;
        chat.Initialize();
        /*string messageText = TrimMessage(chat.firstMessage.Message);
        senderName.text = chat.sender;
        message.text = messageText;*/
    }

    /*private string TrimMessage(string message)
    {
        var index = message.IndexOf(Environment.NewLine);
        if (index < 0 || index > maxMessageLength)
        {
            index = Math.Min(message.Length, maxMessageLength);
        }
        return message.Substring(0, index) + '\u2026';
    }*/
}
