using UnityEngine;

public class ChatButton : MonoBehaviour
{
    void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
