using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField]Transform player;
    Vector3 offset;
    public float SmoothTime = 10f;

    private void Start()
    {
        offset = transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, newPos, SmoothTime * Time.deltaTime);
    }
}