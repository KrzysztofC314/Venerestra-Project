using UnityEngine;

public class MovableBackground : MonoBehaviour
{
    public float windowPosition;
    public float cameraSpeedFraction;

    public virtual void Update()
    {
        var cameraPosition = Camera.main.transform.position.x;
        transform.position = new Vector3(AdjustX(cameraPosition), transform.position.y, transform.position.z);
    }

    private float AdjustX(float cameraPosition)
    {
        return (1 - cameraSpeedFraction) * windowPosition + cameraSpeedFraction * cameraPosition;
    }
}