using System;
using UnityEngine;

public class CursorSettings : MonoBehaviour
{
    public static CursorSettings Instance;
    public Texture2D normalCursor;
    public Texture2D[] activatedCursor;
    public int width;
    public int height;
    private int currentFrame;
    private float frameTimer;
    private float frameRate = .1f;
    public Vector2 hotSpot;

    void Start()
    {
        //normalCursor.Reinitialize(width, height);
        //activatedCursor.Reinitialize(width, height);
        SetCursorActive(false);
        Instance = this;
    }

    void Update()
    {
        Run();
    }

    private Action Run; 

    private void UpdateActiveCursor()
    {
        if (frameTimer > frameRate)
        {
            currentFrame = (currentFrame + 1) % activatedCursor.Length;
            Cursor.SetCursor(activatedCursor[currentFrame++], hotSpot, CursorMode.ForceSoftware);
            frameTimer = 0;
        }
        else
        {
            frameTimer += Time.deltaTime;
        }

    }

    public void SetCursorActive(bool active)
    {
        if (active)
        {
            Run = UpdateActiveCursor;
        }
        else
        {
            Cursor.SetCursor(normalCursor, hotSpot, CursorMode.ForceSoftware);
            Run = () => { };
        }
    }
}
