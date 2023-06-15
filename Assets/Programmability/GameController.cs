

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController Instance => instance is null || !instance.enabled ? Instantiate() : instance;
    private static GameController instance;
    public int MasterVolume;
    public int ResolutionX;
    public int ResolutionY;
    public float resolutionX => ResolutionX / 100f;
    public float resolutionY => ResolutionY / 100f;
    public int Day { get; private set; }
    private GameContext context;
    public GameContext Context => context ??= LoadContext();
    private DayContext dayContext;
    
    public Dictionary<Type, GameObject> Playables = new();
    public GameObject Computer;
    public GameObject Blobchat;
    public List<Person> people = new();
    public GameObject itemBubblePrefab;
    public Weather currentWeather = Weather.DayGoodWeather;
    public const KeyCode useItemKey = KeyCode.R;
    private Dictionary<PlayableObject, Action> onPauseActionDictionary;
    public GameObject pauseScreenPrefab;
    private GameObject pauseScreen;
    private bool gamePaused = false;

    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
                Unpause();
            else
                Pause();
        }
    }

    private void InitPlayablesIfNecessary()
    {
        /*if (Playables.Count == 0)
        {
            Playables.Add(typeof(Computer), Computer);
        }*/
        Playables.TryAdd(typeof(Computer), Computer);
        Playables.TryAdd(typeof(Blobchat), Blobchat);
    }

    

    private static GameController Instantiate()
    {
        var obj = Instantiate(new GameObject());
        if (instance is null || !instance.enabled)
            instance = obj.AddComponent<GameController>();
        return instance;
    }

    public bool Play<T>() where T : PlayableObject
    {
        InitPlayablesIfNecessary();
        PlayableObject.DeleteNulls();
        foreach (var playable in PlayableObject.Collection)
        {
            if (playable is T && playable.transform != null && playable.CanBePlayed)
            {
                playable.Play();
                return true;
            }
        }
        var type = typeof(T);
        if (Playables.ContainsKey(type))
        {
            var prefab = Playables[type];
            var pos = Camera.main.transform.position + new Vector3(0, 0, 20);
            var instance = Instantiate(prefab, pos, Camera.main.transform.rotation);
            instance.GetComponent<T>().Play();
            return true;
        }
        return false;
    }

    private bool CanChangeDay()
    {
        return false;
    }

    public void ChangeDay()
    {
        if (CanChangeDay())
        {
            LoadDay(++Day);
        }
    }

    private GameContext LoadContext()
    {
        return new GameContext();
    }

    private void LoadDay(int day) 
    {
        string dayInfo;
        using (var reader = new StreamReader(Context[day]))
        {
            dayInfo = reader.ReadToEnd();
        }
        dayContext = JsonUtility.FromJson<DayContext>(dayInfo);
    }

    private void Pause()
    {
        onPauseActionDictionary = PlayableObject.Collection.ToDictionary(x => x, x => x.Run);
        foreach (var obj in PlayableObject.Collection)
        {
            obj.Run = () => { };
        }
        pauseScreen = Instantiate(pauseScreenPrefab, Camera.main.transform.position + Vector3.forward, Quaternion.identity, Camera.main.transform);
        gamePaused = true;
    }

    private void Unpause()
    {
        foreach (var obj in PlayableObject.Collection)
        {
            obj.Run = onPauseActionDictionary[obj];
        }
        Destroy(pauseScreen);
        gamePaused = false;
    }
}