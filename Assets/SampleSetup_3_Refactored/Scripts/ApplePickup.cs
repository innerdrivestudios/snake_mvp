using UnityEngine;

public class ApplePickup : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 60;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
