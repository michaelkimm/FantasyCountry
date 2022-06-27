using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel_SeatsPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localRotation = Quaternion.FromToRotation(-transform.parent.up, Vector3.down);
    }
}
