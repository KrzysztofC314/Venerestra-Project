using UnityEngine;

public class RoomChangeTrigger : MonoBehaviour
{
    public GameObject targetRoom;
    public int spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player == null)
            return;
        var thisRoom = GetComponentInParent<GenericRoom>().gameObject;
        var nextRoom = Instantiate(targetRoom, Vector3.zero, Quaternion.identity).GetComponent<GenericRoom>();
        nextRoom.Initialize(spawnPoint);
        Destroy(thisRoom);
    }
}
