using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;
    public Transform target;
    public Vector2 MaxPos, MinPos;

    private void Update()
    {
        GameObject bullet = GameObject.FindGameObjectWithTag("Bullet");
        if (bullet) target = bullet.transform;
    }
    void LateUpdate()
    {
        if (target)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
                targetPos.x = Mathf.Clamp(targetPos.x, MinPos.x, MaxPos.x);
                targetPos.y = Mathf.Clamp(targetPos.y, MinPos.y, MaxPos.y);
                transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);
            }
        }
       
        
    }
}
