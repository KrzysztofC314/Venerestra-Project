using UnityEngine;

public class GenericRoom : MonoBehaviour
{
    public Vector3[] startingPoints;
    public int width;
    public int height;

    public virtual void Initialize(int pointNo)
    {
        PlayerMovement.Instance.transform.position = transform.position + startingPoints[pointNo];
        CameraFollow.Instance.Initialize(width, height);
    }
}
