using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Computer : PlayableObject
{
    public Dictionary<OnClickBehaviourPattern, Action> Behave => behave ??= GetBehaviour();
    private Dictionary<OnClickBehaviourPattern, Action> behave;
    private Action DoNothing => () => { };
    private Action Quit => SwitchPlayable<PlayerMovement>;
    private Action OpenChat => SwitchPlayable<Blobchat>;
    private ComputerElement ActiveElement;
    public Vector2 Size;
    public LayerMask elementLayerMask = 6;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        CameraFollows = false;
    }

    private void BaseUpdate()
    {
        var pos = Input.mousePosition;
        var worldPos = (Vector2)(Camera.main.ScreenToWorldPoint(pos));
        var collider = Physics2D.OverlapPoint(worldPos, elementLayerMask);
        Debug.Log($"{worldPos}, active: {ActiveElement != null}, collider: {collider != null}");

        if (collider != null)
        {
            var element = collider.GetComponent<ComputerElement>();
            Activate(element);
        }
        else
        {
            Deactivate();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (ActiveElement != null)
            {
                ActiveElement.Clicked();
            }
            else
            {
                collider = Physics2D.OverlapPoint(worldPos, elementLayerMask);
                if (collider != null && collider.GetComponent<ComputerExitButton>() != null)
                {
                    SwitchPlayable<PlayerMovement>();
                }
            }

            Clicked(pos.x, pos.y);
        }
    }

    private void Activate(ComputerElement element)
    {
        if (element != null)
        {
            ActiveElement = element;
            ActiveElement.Activate();
        }
    }

    private void Deactivate()
    {
        if (ActiveElement != null)
        {
            ActiveElement.Deactivate();
            ActiveElement = null;
        }
    }

    private void Clicked(float x, float y)
    {
        Debug.Log($"klikniêto: ({x}, {y})");
    }

    public override void Play()
    {
        base.Play();

        var vertices = GetComponent<SpriteRenderer>().sprite.vertices;
        var x = Math.Abs(vertices.Select(v => v.x).Sum() / 4.0f - vertices[0].x) * 2;
        var y = Math.Abs(vertices.Select(v => v.y).Sum() / 4.0f - vertices[0].y) * 2;
        Size = new Vector2(x, y);

        Run = BaseUpdate;
    }

    public override void Stop()
    {
        Destroy(gameObject);
    }

    public Dictionary<OnClickBehaviourPattern, Action> GetBehaviour()
    {
        var behaviourDictionary = new Dictionary<OnClickBehaviourPattern, Action>
        {
            { OnClickBehaviourPattern.None, DoNothing },
            { OnClickBehaviourPattern.Quit, Quit },
            { OnClickBehaviourPattern.OpenChat, OpenChat }
        };
        return behaviourDictionary;
    }

    public enum OnClickBehaviourPattern
    {
        None,
        Quit,
        OpenChat
    }
}
