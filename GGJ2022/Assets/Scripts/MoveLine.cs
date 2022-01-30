using UnityEngine;
using System.Collections;

enum LineState {
    FOLLOW,
    STATIC
}

public class MoveLine : MonoBehaviour {    
    LineRenderer _line;
    [SerializeField] Vector3 _startPos;
    [SerializeField] Vector3 _endPos;
    [SerializeField] Transform _follow;
    [SerializeField] LineState _state;

    void Start ()
    {
        _line = gameObject.GetComponent<LineRenderer>();
    }

    public void Init(Vector3 startPos, Transform follow) {
        _startPos = startPos;
        _follow = follow;
        _state = LineState.FOLLOW;
    }

    public Vector3 Lock() {
        _endPos = _follow.position;
        _line.SetPosition(0, _startPos);
        _line.SetPosition(1, _endPos);
        _state = LineState.STATIC;
        return _endPos;
    }

    public void Unlock() {
        _state = LineState.FOLLOW;
    }

    public float Distance() {
        return Vector3.Distance(_line.GetPosition(0), _line.GetPosition(1));
    }

    void Update()
    {
        if(_state == LineState.FOLLOW && _follow) {
            _line.SetPosition(0, _startPos);
            _line.SetPosition(1, _follow.position);  
        }
    }
}