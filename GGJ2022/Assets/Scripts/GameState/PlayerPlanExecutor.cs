using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlanExecutor: MonoBehaviour {
	public bool IsDone { get; set; }
	[SerializeField] Player player;

	void Start() {
	}

	void OnEnable() {
		IsDone = false;
	}

	public void ExecutePlan(List<Command> commands) {
		StartCoroutine(RunCommands(commands, 0));
	}

	IEnumerator RunCommands(List<Command> commands, int index) {
		if(index >= commands.Count) {
			IsDone = true;
			yield break;
		}

		if(commands[index].Type == CommandType.MOVE) {
			MoveCommand mcommand = (MoveCommand)commands[index];
			float dist = Vector3.Distance(transform.position, mcommand.EndPosition);
			float maxTime = dist/player.PlayerSpeed;
			float accTime = 0f;
			// Move to the target location
			while(Vector3.Distance(transform.position, mcommand.EndPosition) >= player.CharacterController.radius && accTime < maxTime)
	        {
	        	player.transform.forward = (mcommand.EndPosition - transform.position).normalized;
	        	player.MyCharacterController.Move(transform.forward * Time.fixedDeltaTime * player.PlayerSpeed);
	        	accTime += Time.fixedDeltaTime;
	            yield return new WaitForFixedUpdate();
	        }
		}
		else {
			AttackCommand acommand = (AttackCommand)commands[index];
			player.transform.forward = acommand.Direction;
			switch(acommand.AttackName) {
				case "1":
					player.DoAbility1();
					break;
				case "2":
					player.DoAbility2();
					break;
				case "3":
					player.DoAbility3();
					break;
				case "4":
					player.DoUltimateAbility();
					break;
			}

			while(true) {
				yield return new WaitForSeconds(1f);
				if(!player.IsUsingAbility) {
					break;
				}
			}
		}

		StartCoroutine(RunCommands(commands, index+1));
		yield return null;
	}
}