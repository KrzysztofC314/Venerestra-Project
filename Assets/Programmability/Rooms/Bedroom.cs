using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bedroom : GenericRoom
{
    public GameObject nightDark;
    public GameObject nightLight;
    public GameObject dayBadWeather;
    public GameObject dayGoodWeather;

    public void LoadScenery()
    {
        UnloadScenery();

        GameObject sceneryPrefab = null;
        switch (GameController.Instance.currentWeather)
        {
            case Weather.NightDark:
                sceneryPrefab = nightDark;
                break;
            case Weather.NightLight:
                sceneryPrefab = nightLight;
                break;
            case Weather.DayBadWeather:
                sceneryPrefab = dayBadWeather;
                break;
            case Weather.DayGoodWeather:
                sceneryPrefab = dayGoodWeather;
                break;
        }

        Instantiate(sceneryPrefab, transform);
    }

    public void UnloadScenery()
    {
        var sprite = GetComponentInChildren<RoomSprite>();
        if (sprite != null)
            Destroy(sprite.gameObject);
    }

    public override void Initialize(int pointNo)
    {
        base.Initialize(pointNo);
        LoadScenery();
    }
}
