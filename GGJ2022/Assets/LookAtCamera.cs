using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Quaternion quat = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up);
            Debug.Log(quat.eulerAngles);

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up);
    }
}
