using System;
using System.Collections.Generic;
using UnityEngine;

public class ComputerElement : MonoBehaviour
{
    private Computer Computer;
    public Computer.OnClickBehaviourPattern Behaviour = Computer.OnClickBehaviourPattern.None;
    public Action Clicked => Computer.Behave[Behaviour];
    private Animator Animator;

    private void Start()
    {
        Computer = GetComponentInParent<Computer>();
        Animator = GetComponent<Animator>();
    }

    internal void SetPosition(int index, int count)
    {
        int cols = (int)Math.Ceiling(Math.Sqrt(count));
        int rows = (int)Math.Ceiling(count / (float)cols);
        int column = index % cols;
        int row = index / cols;
        float width = Computer.Size.x / cols;
        float height = Computer.Size.y / rows;
        float furthestX = (cols - 1) / 2.0f * width;
        float furthestY = (rows - 1) / 2.0f * height;
        float x = Computer.transform.position.x - furthestX + column * width;
        float y = Computer.transform.position.y- furthestY + row * height;
        transform.position = new Vector2(x, y);
    }

    public void Activate()
    {
        SetActive(true);
    }

    public void Deactivate()
    {
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        Animator.SetBool("isActive", active);
    }
}
