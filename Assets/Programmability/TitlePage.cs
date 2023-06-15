using UnityEngine;

public class TitlePage : MonoBehaviour
{
    public GameObject menu;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Instantiate(menu, Vector3.zero, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
