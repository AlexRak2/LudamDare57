using UnityEngine;

namespace LD57
{
public class MenuCameraRotator : MonoBehaviour
{
    [SerializeField] private Transform roatationPoint;
    Quaternion startRotation;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float rotationAmount = 10f;

    private void Start()
    {
        startRotation = roatationPoint.rotation;
    }
    private void Update()
    {
        roatationPoint.rotation = startRotation * Quaternion.Euler(0, Mathf.Sin(Time.time * rotationSpeed) * rotationAmount, 0);
    }
}
}