using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableObject : MonoBehaviour
{
    public Camera usableCamera;
    protected CameraFollow _camera => _Camera ??= CameraFollow.Instance;
    private CameraFollow _Camera;
    public static List<PlayableObject> Collection = new List<PlayableObject>();
    private bool initialized = false;
    public virtual Vector3 cameraSpawnPosition => transform.position;

    public virtual void Start()
    {
        if (initialized) return;
        usableCamera = Camera.main;
        Collection.Add(this);
        initialized = true;
    }
    public abstract void Stop();
    protected bool CameraFollows { get; set; }
    public Action Run = () => { };

    public bool CanBePlayed { get; protected set; } = true;

    public virtual void Play()
    {
        Start();
        _camera.StopAndSetPosition(cameraSpawnPosition);
        if (CameraFollows)
            _camera.Follow(transform);
        else
            _camera.Unfollow();
    }

    public virtual void Update()
    {
        Run();
    }

    protected void TurnOff()
    {
        Run = () => { };
    }

    protected void UseItem()
    {
        ItemBehaviour.Use(this);
    }

    protected internal void SwitchPlayable<T>() where T : PlayableObject
    {
        if (GameController.Instance.Play<T>())
        {
            CursorSettings.Instance.SetCursorActive(false);
            Stop();
        }
    }

    public static void DeleteNulls()
    {
        for (int i = Collection.Count - 1; i >= 0; i--)
        {
            if (Collection[i] == null)
            {
                Collection.RemoveAt(i);
            }
        }
    }
}