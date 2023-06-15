using System;
using System.IO;
using UnityEngine;

public class GameContext
{
    public readonly string[] DayFilePaths;

    public GameContext()
    {
        DayFilePaths = InitializeFilePaths();
    }

    public string this[int key] => DayFilePaths[key];

    private string[] InitializeFilePaths()
    {
        string info;
        var guwno = Application.dataPath;
        using (var reader = new StreamReader(Application.dataPath + "/Data/fsinfo.dat"))
        {
            info = reader.ReadToEnd();
        }
        return info.Split(Environment.NewLine);
    }
}