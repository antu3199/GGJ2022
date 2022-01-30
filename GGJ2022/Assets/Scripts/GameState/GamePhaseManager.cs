using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GamePhase {
	FREE_ROAM,
	PLAN,
	EXECUTE
}

// Phases in Order
//
// FREE_ROAM --> PLAN --> EXECUTE --> FREE_ROAM ...

public class GamePhaseManager: Global<GamePhaseManager> 
{
	[SerializeField] GamePhase _currentPhase;
	[SerializeField] PauseController _pauseController;
	[SerializeField] Dictionary<GameObject, RechargeableGauge> _gauges = new Dictionary<GameObject, RechargeableGauge>();

	void Start()
	{
		_pauseController = (PauseController)PauseController.Instance;
		GoFreeRoamPhase();
	}

	bool GoFreeRoamPhase() {
		_pauseController.ResetAll();
		foreach(KeyValuePair<GameObject, RechargeableGauge> gauge in _gauges) {
			gauge.Value.ResetGauge();	
		}
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
		_currentPhase = GamePhase.PLAN;
		return true;
	}

	bool GoExecutePhase() {
		_pauseController.SlowAll();
		foreach(KeyValuePair<GameObject, RechargeableGauge> gauge in _gauges) {
			gauge.Value.PauseGauge();
		}
		_currentPhase = GamePhase.EXECUTE;
		return true;
	}
}