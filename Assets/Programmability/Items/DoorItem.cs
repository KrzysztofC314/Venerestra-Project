using UnityEngine;

public class DoorItem : ItemBehaviour
{
    public GameObject targetRoom;
    public int spawnPoint;

    protected override void UseItem(PlayableObject sender)
    {
        var thisRoom = GetComponentInParent<GenericRoom>().gameObject;
        var nextRoom = Instantiate(targetRoom, Vector3.zero, Quaternion.identity).GetComponent<GenericRoom>();
        nextRoom.Initialize(spawnPoint);
        Destroy(thisRoom);
    }
}
