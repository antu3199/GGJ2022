using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform trackedPlayer;
    public Vector3 offset;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, trackedPlayer.position + offset, 3 * Time.deltaTime);
    }
}
