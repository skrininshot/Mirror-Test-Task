using UnityEngine;

public class SpaceCanvas : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.current;
    }

    private void Update()
    {
        if (Camera.current == null) return;

        transform.rotation = Camera.current.transform.rotation;
    }
}
