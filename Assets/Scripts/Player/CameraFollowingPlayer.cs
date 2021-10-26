using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingPlayer : MonoBehaviour
{
    public Camera cam;

    [SerializeField] float smoothness = 15f;

    Vector3 velocity = Vector3.zero;

    private void Update()
    {
        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, -10);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, playerPosition, ref velocity, smoothness);
    }
}
