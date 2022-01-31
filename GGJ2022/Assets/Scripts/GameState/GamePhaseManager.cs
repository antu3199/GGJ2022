using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase {
	FREE_ROAM,
	PLAN,
	EXECUTE
}

// Phases in Order
//
// FREE_ROAM --> PLAN --> EXECUTE --> FREE_ROAM ...

public class GamePhaseManager: Global<GamePhaseManager> 
{
	public GamePhase _currentPhase;
	[SerializeField] PauseController _pauseController;
	[SerializeField] Dictionary<GameObject, RechargeableGauge> _gauges = new Dictionary<GameObject, RechargeableGauge>();
	
	[SerializeField] KeyCode _toNextPhase = KeyCode.Space;
	[SerializeField] HashSet<AbilityBar> _abilities = new HashSet<AbilityBar>();

	void Start()
	{
		_pauseController = (PauseController)PauseController.Instance;
		GoFreeRoamPhase();
	}

	void Update() 
	{
		if(_currentPhase == GamePhase.EXECUTE && CheckAllExecutionComplete()) {
			GoFreeRoamPhase();
		}
		if(Input.GetKeyDown(_toNextPhase)) {
			if(_currentPhase == GamePhase.FREE_ROAM) {
				GoPlanPhase();
			}
			else if(_currentPhase == GamePhase.PLAN) {
				GoExecutePhase();
			}
		}
	}

	public void Attach(AbilityBar bar) 
	{
		if(!_abilities.Contains(bar)) {
			_abilities.Add(bar);
		}
	}

	public void Detach(AbilityBar bar)
	{
		if(_abilities.Contains(bar)) {
			_abilities.Remove(bar);
		}
	}	

	bool GoFreeRoamPhase() {
		_pauseController.ResetAll();
		foreach(KeyValuePair<GameObject, RechargeableGauge> gauge in _gauges) {
			gauge.Value.ResetGauge();	
		}
		ToPlayerFreeMove();
		_currentPhase = GamePhase.FREE_ROAM;
		return true;
	}

	bool GoPlanPhase() {
		bool allGaugesMaxed = true;
		foreach(KeyValuePair<GameObject, RechargeableGauge> gauge in _gauges) {
			allGaugesMaxed = allGaugesMaxed & gauge.Value.IsMaxed();
		}
		if(!allGaugesMaxed) { return false; }
		_pauseController.PauseAll();
		ToPlayerPlanning();
		_currentPhase = GamePhase.PLAN;
		return true;
	}

	bool GoExecutePhase() {
		_pauseController.SlowAll();
		foreach(KeyValuePair<GameObject, RechargeableGauge> gauge in _gauges) {
			gauge.Value.PauseGauge();
		}
		ToPlayerExecutePlan();
		_currentPhase = GamePhase.EXECUTE;
		return true;
	}

	void ToPlayerFreeMove() {
		foreach(AbilityBar ab in _abilities) {
			ab.Player.DisablePlayerInteraction = false;
			ab.Player.Executor.gameObject.SetActive(false);
			ab.enabled = true;
		}
	}

	void ToPlayerPlanning() {
		foreach(AbilityBar ab in _abilities) {
			ab.Player.DisablePlayerInteraction = true;
			ab.Player.Planner.gameObject.SetActive(true);
			ab.enabled = false;
		}
	}	

	void ToPlayerExecutePlan() {
		foreach(AbilityBar ab in _abilities) {
			Player player = ab.Player;
			player.Executor.gameObject.SetActive(true);
			player.Executor.ExecutePlan(player.Planner.GetPlan());
			player.Planner.gameObject.SetActive(false);
		}
	}

	bool CheckAllExecutionComplete() {
		bool isDone = true;
		foreach(AbilityBar ab in _abilities) {
			Player player = ab.Player;
			isDone = isDone && player.Executor.IsDone;
		}
		return isDone;
	}
}