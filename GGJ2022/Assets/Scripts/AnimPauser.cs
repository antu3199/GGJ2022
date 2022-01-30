using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPauser: MonoBehaviour, IPausable 
{
	protected Animator m_anim;

	void Start()
	{
		m_anim = GetComponent<Animator>();
		AttachPausable();
	}

	void OnDestroy() 
	{
		DetachPausable();
	}

	// Impl Interface
	public void AttachPausable()
	{
		PauseController pauser = (PauseController)PauseController.Instance;
        pauser?.Attach(this.gameObject, this);
	}

	public void DetachPausable()
	{
		PauseController pauser = (PauseController)PauseController.Instance;
        pauser?.Detach(this.gameObject, this);
	}

	public void Pause()
	{
		m_anim.speed = 0;
	}

	public void Slow(float percentage)
	{
		m_anim.speed = percentage;
	}

	public void Reset()
	{
		m_anim.speed = 1;
	}
}