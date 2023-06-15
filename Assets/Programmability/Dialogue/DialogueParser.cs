using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueParser
{
    private readonly string FileName;
    private string FileContents;
    private const string TitleStart = ":: ";
    private readonly string MessageStart = Environment.NewLine;
    private const string ResponseStart = "[[";
    private const string ResponseEnd = "]]";
    private Dictionary<string, Dialogue> Dialogues = new();

    public DialogueParser(string fileName)
    {
        FileName = fileName;
    }

    public Dialogue Parse()
    {
        try
        {
            using (var reader = new StreamReader(Application.dataPath + FileName))
            {
                FileContents = reader.ReadToEnd();
            }
        }
        catch (IOException ioe)
        {
            Console.WriteLine(ioe);
            throw ioe;
        }
        string startNodeTitle = ExtractStartNodeTitle();
        int start = FileContents.IndexOf(":: UserStylesheet") + 1;
        Parse(start);
        AddLinks();
        return Dialogues[startNodeTitle];
    }

    private void Parse(int start)
    {
        start = FileContents.IndexOf(TitleStart, start);
        if (start < 0)
            return;
        start += TitleStart.Length;
        string title = FileContents.Substring(start, FileContents.IndexOf("{", start) - start).Trim();
        start = FileContents.IndexOf(MessageStart, start) + MessageStart.Length;
        int responseStart = FileContents.IndexOf(ResponseStart, start);
        int end = FileContents.IndexOf(TitleStart, start);
        int next = Next(responseStart, end);
        if (next == -1)
            next = FileContents.Length - 1;
        string message = FileContents.Substring(start, next - start).Trim();
        start = responseStart + ResponseStart.Length;
        var responses = new List<Response>();
        if (responseStart != -1)
            while (start < end)
            {
                string responseString = FileContents.Substring(start, FileContents.IndexOf(ResponseEnd, start) - start).Trim();
                var responseSplit = responseString.Split("->");
                Response response = new Response
                {
                    Message = responseSplit[0],
                    ActionString = responseSplit.Length > 1 ? responseSplit[1] : responseSplit[0]
                };
                responses.Add(response);
                start = FileContents.IndexOf(ResponseStart, start);
                if (start == -1)
                    break;
                start += ResponseStart.Length;
            }
        var messages = message.Split(Environment.NewLine);
        Dialogue dialogue = null;
        for (int i = 0; i < messages.Length; i++)
        {
            var senderAndMessage = messages[i].Split(":");
            dialogue = new Dialogue
            {
                Sender = senderAndMessage.Length > 1 ? senderAndMessage[0].Trim() : string.Empty,
                Message = senderAndMessage.Length > 1 ? senderAndMessage[1].Trim() : senderAndMessage[0].Trim(),
                PreviousMessageInThisPassage = dialogue
            };
        }
        dialogue.Title = title;
        dialogue.Responses = responses.ToArray();
        Dialogues.Add(title, dialogue);
        Parse(Math.Max(end, next));
    }

    private void AddLinks()
    {
        foreach (var dialogue in Dialogues.Values)
        {
            foreach (var response in dialogue.Responses)
            {
                response.Action = Dialogues[response.ActionString];
            }
        }
    }

    private string ExtractStartNodeTitle()
    {
        string startMarker = "\"start\": \"";
        int startPosition = FileContents.IndexOf(startMarker) + startMarker.Length;
        int length = FileContents.IndexOf("\"", startPosition) - startPosition;
        return FileContents.Substring(startPosition, length);
    }

    private int Next(int x, int y)
    {
        if (x == -1)
            return y;
        if (y == -1)
            return x;
        return Math.Min(x, y);
    }
}