using UnityEngine;

public class Beach : GenericRoom
{
    public GameObject sadBeach;
    public GameObject lessSadBeach;
    public GameObject almostHappyBeach;
    public GameObject happyBeach;

    public enum BeachMood
    {
        Sad,
        LessSad,
        AlmostHappy,
        Happy
    }

    public void LoadScenery(BeachMood mood)
    {
        UnloadScenery();

        GameObject sceneryPrefab = null;
        switch (mood)
        {
            case BeachMood.Sad:
                sceneryPrefab = sadBeach;
                break;
            case BeachMood.LessSad:
                sceneryPrefab = lessSadBeach;
                break;
            case BeachMood.AlmostHappy:
                sceneryPrefab = almostHappyBeach;
                break;
            case BeachMood.Happy:
                sceneryPrefab = happyBeach;
                break;
        }

        Instantiate(sceneryPrefab, transform.position, transform.rotation, transform);
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
        LoadScenery(BeachMood.Sad);
    }
}
