using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlanExecutor: MonoBehaviour {
	Player player;

	void Start() {
		player = gameObject.GetComponentInParent<Player>();
	}

	public void ExecutePlan(List<Command> commands) {
		StartCoroutine(RunCommands(commands, 0));
	}

	IEnumerator RunCommands(List<Command> commands, int index) {
		if(index >= commands.Count) {
			yield break;
		}

		if(commands[index].Type == CommandType.MOVE) {
			MoveCommand mcommand = (MoveCommand)commands[index];
			float dist = Vector3.Distance(transform.position, mcommand.EndPosition);
			float maxTime = dist/player.PlayerSpeed;
			float accTime = 0f;
			// Move to the target location
			while(Vector3.Distance(transform.position, mcommand.EndPosition) >= 0.1f && accTime < maxTime)
	        {
	        	player.transform.forward = (mcommand.EndPosition - transform.position).normalized;
	        	player.MyCharacterController.Move(transform.forward * Time.fixedDeltaTime * player.PlayerSpeed);
	            yield return new WaitForFixedUpdate();
	        }
		}
		else {
			// TODO Perform Attack
		}

		StartCoroutine(RunCommands(commands, index));
		yield return null;
	}
}