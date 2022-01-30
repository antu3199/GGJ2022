using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType {
	MOVE,
	ATTACK
}

public abstract class Command {
	public CommandType Type;
	public abstract void Lock();
}

public class MoveCommand: Command {
	public Vector3 StartPosition;
	public Vector3 EndPosition;
	public MoveLine LineRenderer;

	public MoveCommand(Vector3 startPos, GameObject obj, Transform follow) {
		StartPosition = startPos;
		LineRenderer = obj.GetComponent<MoveLine>();
		LineRenderer.Init(startPos, follow);
	}

	public override void Lock() {
		LineRenderer.Lock();
	}
}

public class AttackCommand: Command {
	public string AttackName;
	public Vector3 Position;
	public Vector3 Direction;
	public GameObject AttackIndicator;

	public AttackCommand(Vector3 position, Vector3 direction, GameObject obj, string attackName) {
		Position = position;
		Direction = direction;
		AttackIndicator = obj;
		AttackName = attackName;
	}

	public override void Lock() {
		AttackIndicator.transform.position = Position;
		AttackIndicator.transform.forward = Direction;
	}
}

public class PlayerPlanSetter: MonoBehaviour {	
	// Move Ghost Objects	
	protected CharacterController _cc;
	[SerializeField] protected float PlayerSpeed = 3.0f;
	// KeyCommand Overrides
	[SerializeField] protected KeyCode ForwardKeyCode = KeyCode.UpArrow;
    [SerializeField] protected KeyCode BackwardKeyCode = KeyCode.DownArrow;
    [SerializeField] protected KeyCode LeftKeyCode = KeyCode.LeftArrow;
    [SerializeField] protected KeyCode RightKeyCode = KeyCode.RightArrow;
    [SerializeField] protected KeyCode LockMoveCommandCode = KeyCode.P;
    [SerializeField] protected KeyCode LockAttack1CommandCode = KeyCode.Keypad7;
    [SerializeField] protected KeyCode LockAttack2CommandCode = KeyCode.Keypad8;
    [SerializeField] protected KeyCode LockAttack3CommandCode = KeyCode.Keypad9;
    [SerializeField] protected KeyCode LockAttack4CommandCode = KeyCode.Keypad0;

    // Commands
    List<Command> _commands = new List<Command>();
    Command _currentCommand = null;

    [SerializeField] GameObject _movePrefab;
    [SerializeField] GameObject _attackPrefab;

	void Start()
    {
    	_cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovementHandler();
    }

    void OnEnable() {
    	Reset();
    	CreateCommand(CommandType.MOVE);
    }

    void OnDisable() {
    	Reset();
    }

    public List<Command> GetCommands() 
    {
    	return _commands;
    }

    public void Reset() {
    	transform.localPosition = Vector3.zero;
    	_commands.Clear();
    }

    public void CreateCommand(CommandType type, string attackName = "") {
    	if(type == CommandType.MOVE) {
    		GameObject obj = (GameObject)Instantiate(_movePrefab, transform.parent);
    		_currentCommand = new MoveCommand(transform.position, obj, transform);
    	}
    	else if(type == CommandType.ATTACK) {
    		_currentCommand.Lock(); //Lock an existing move command
    		GameObject obj = (GameObject)Instantiate(_attackPrefab, transform.parent);
    		_currentCommand = new AttackCommand(transform.position, transform.forward, obj, attackName);
    	}
    }

    public void LockInCommand() {
    	if(_currentCommand == null) { return; }
    	_currentCommand.Lock();
    	_commands.Add(_currentCommand);
    	CreateCommand(CommandType.MOVE);
    }

    private void MovementHandler() {
    	// Yoinked from https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
        if (_cc == null) return;

        float vertical = 0;
        float horizontal = 0;

        if (Input.GetKey(ForwardKeyCode)) {
            vertical = 1;
        }

        if (Input.GetKey(BackwardKeyCode)) {
            vertical = -1;
        }

        if (Input.GetKey(RightKeyCode)) {
            horizontal = 1;
        }

         if (Input.GetKey(LeftKeyCode)) {
            horizontal = -1;
        }

        if(Input.GetKeyDown(LockMoveCommandCode)) {
        	LockInCommand();
        }

        if(Input.GetKeyDown(LockAttack1CommandCode)) {
        	CreateCommand(CommandType.ATTACK, "1");
        	LockInCommand();
        }
        if(Input.GetKeyDown(LockAttack2CommandCode)) {
        	CreateCommand(CommandType.ATTACK, "2");
        	LockInCommand();
        }
        if(Input.GetKeyDown(LockAttack3CommandCode)) {
        	CreateCommand(CommandType.ATTACK, "3");
        	LockInCommand();
        }
        if(Input.GetKeyDown(LockAttack4CommandCode)) {
        	CreateCommand(CommandType.ATTACK, "4");
        	LockInCommand();
        }

        Vector3 moveVector = new Vector3(horizontal, 0, vertical);

        _cc.Move(moveVector * Time.deltaTime * PlayerSpeed);

        if (moveVector != Vector3.zero) {
            gameObject.transform.forward = moveVector;
        }
    }
}