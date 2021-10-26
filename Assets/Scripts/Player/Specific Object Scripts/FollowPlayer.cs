using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] float offsetY;
    [SerializeField] float offsetX;

    private void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x -offsetX, target.position.y - offsetY, target.position.z);
        transform.position = targetPosition;
    }
}
