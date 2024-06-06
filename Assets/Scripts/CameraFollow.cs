using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    [SerializeField] Transform target;
    
    void LateUpdate () 
    {
        if (transform == null) 
            return;
        
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
    }
}
