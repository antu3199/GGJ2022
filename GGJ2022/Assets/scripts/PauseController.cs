using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController: Global
{
    // Public Members
    public float m_slowAnimSpeed;

    // Private Members
    private HashSet<GameObject> m_animatedObjects = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            ResetAll();
        }
        if (Input.GetKeyDown("s"))
        {
            SlowAll();
        }
        if (Input.GetKeyDown("d"))
        {
            PauseAll();
        }
    }

    // Let the Pause Controller know that obj can be paused
    public void Attach(GameObject obj) {
        m_animatedObjects.Add(obj);
    }

    // On GameObject removal, remove the obj reference from the Pause Controller
    public void Detach(GameObject obj) {
        if(m_animatedObjects.Contains(obj)) {
            m_animatedObjects.Remove(obj);
        }
        else {
            Debug.LogWarning("Object not tracked by pauser attempting to be removed.");
        }
    }

    public void PauseAll() {
        foreach(GameObject entry in m_animatedObjects) {
            Animator anim = entry.GetComponent<Animator>();
            if(anim != null) {
                anim.speed = 0;
            }
        }
    }

    public void SlowAll() {
        foreach(GameObject entry in m_animatedObjects) {
            Animator anim = entry.GetComponent<Animator>();
            if(anim != null) {
                anim.speed = m_slowAnimSpeed;
            }
        }
    }

    public void ResetAll() {
        foreach(GameObject entry in m_animatedObjects) {
            Animator anim = entry.GetComponent<Animator>();
            if(anim != null) {
                anim.speed = 1;
            }
        }
    }

    public void Pause(GameObject obj) {
        if(m_animatedObjects.Contains(obj)) {
            Animator anim = obj.GetComponent<Animator>();
            anim.speed = 0;
        }
        else {
            Debug.LogWarning("Object not tracked by pauser attempting to be paused.");
        }
    }

    public void Slow(GameObject obj) {
        if(m_animatedObjects.Contains(obj)) {
            Animator anim = obj.GetComponent<Animator>();
            anim.speed = m_slowAnimSpeed;
        }
        else {
            Debug.LogWarning("Object not tracked by pauser attempting to be slowed.");
        }
    }

    public void Reset(GameObject obj) {
        if(m_animatedObjects.Contains(obj)) {
            Animator anim = obj.GetComponent<Animator>();
            anim.speed = 1;
        }
        else {
            Debug.LogWarning("Object not tracked by pauser attempting to be reset.");
        }
    }
}
