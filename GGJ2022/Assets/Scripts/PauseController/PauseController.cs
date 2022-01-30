using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController: Global<PauseController>
{
    // Public Members
    [Range(0f, 1f)]
    public float m_slowPercentage;

    // Private Members
    private Dictionary<GameObject, HashSet<IPausable>> m_pausableObjects = new Dictionary<GameObject, HashSet<IPausable>>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    // Let the Pause Controller know that obj can be paused
    public void Attach(GameObject obj, IPausable pausable) {
        if(!m_pausableObjects.ContainsKey(obj))
        {
            m_pausableObjects.Add(obj, new HashSet<IPausable>());
        }
        m_pausableObjects[obj].Add(pausable);
    }

    // On GameObject removal, remove the obj reference from the Pause Controller
    public void Detach(GameObject obj, IPausable pausable) {
        if(!m_pausableObjects.ContainsKey(obj)) { return; }
        if(!m_pausableObjects[obj].Contains(pausable)) { return; }
        m_pausableObjects[obj].Remove(pausable);        
        if(m_pausableObjects[obj].Count == 0)
        {
            m_pausableObjects.Remove(obj);
        }
    }

    public void PauseAll() {
        foreach(KeyValuePair<GameObject, HashSet<IPausable>> pausables in m_pausableObjects) {
            foreach(IPausable pausable in pausables.Value) {
                pausable.Pause();
            }
        }
    }

    public void SlowAll() {
        foreach(KeyValuePair<GameObject, HashSet<IPausable>> pausables in m_pausableObjects) {
            foreach(IPausable pausable in pausables.Value) {
                pausable.Slow(m_slowPercentage);
            }
        }
    }

    public void ResetAll() {
        foreach(KeyValuePair<GameObject, HashSet<IPausable>> pausables in m_pausableObjects) {
            foreach(IPausable pausable in pausables.Value) {
                pausable.Reset();
            }
        }
    }

    public void PauseGameObject(GameObject obj) {
        if(m_pausableObjects.ContainsKey(obj)) {
            foreach(IPausable pausable in m_pausableObjects[obj]) {
                pausable.Pause();
            }
        }
    }

    public void SlowGameObject(GameObject obj) {
        if(m_pausableObjects.ContainsKey(obj)) {
            foreach(IPausable pausable in m_pausableObjects[obj]) {
                pausable.Slow(m_slowPercentage);
            }
        }
    }

    public void ResetGameObject(GameObject obj) {
        if(m_pausableObjects.ContainsKey(obj)) {
            foreach(IPausable pausable in m_pausableObjects[obj]) {
                pausable.Reset();
            }
        }
    }

    public void Pause(IPausable pausable) {
        pausable.Pause();
    }

    public void Slow(IPausable pausable) {
        pausable.Slow(m_slowPercentage);
    }

    public void Reset(IPausable pausable) {
        pausable.Reset();
    }
}
