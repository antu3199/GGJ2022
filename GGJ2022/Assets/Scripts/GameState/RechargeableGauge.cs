using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rechargeable gauge based on time

public enum GaugeState {
	PAUSED,
	ACTIVE
}

public class RechargeableGauge: MonoBehaviour
{
	[SerializeField] float _chargeSpeed = 1;
	[SerializeField] float _maxCharge;
	[SerializeField] float _currentCharge;
	[SerializeField] GaugeState _state;

	void Start() {
		_state = GaugeState.ACTIVE;
		_currentCharge = 0;
	}

	void Update() {
		if(_state == GaugeState.ACTIVE &&
			_currentCharge < _maxCharge) {
			_currentCharge = Mathf.Min(_currentCharge + Time.deltaTime * _chargeSpeed, _maxCharge);
		}
	}

	public float GetCharge() {
		return _currentCharge;
	}

	public GaugeState GetState() {
		return _state;
	}

	public bool IsMaxed() {
		return _currentCharge == _maxCharge;
	}

	public void PauseGauge() {
		_state = GaugeState.PAUSED;
	}

	public void StartGauge() {
		_state = GaugeState.ACTIVE;
	}

	public void ResetGauge() {
		_state = GaugeState.ACTIVE;
		_currentCharge = 0;
	}

	public bool Consume(float val) {
		if(_currentCharge < val) { return false; }
		_currentCharge -= val;
		return true;
	}
}