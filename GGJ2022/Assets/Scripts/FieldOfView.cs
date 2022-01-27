using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
	// In Degrees
	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;
	public LayerMask[] targetMasks;
	public LayerMask[] obstacleMasks;
	public List<GameObject> Visible = new List<GameObject>();

	[SerializeField] protected float delay = 0.2f;	

	public Vector3 DirFromAngle(float angleInDeg, bool isGlobalAngle) 
	{
		if(!isGlobalAngle) 
		{
			angleInDeg += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
	}

	protected void GetVisibleTargetsHandler() 
	{
		this.Visible.Clear();
		List<Collider> inView = new List<Collider>();
		foreach(LayerMask mask in targetMasks)
		{
			Collider[] localInView = Physics.OverlapSphere(transform.position, viewRadius, mask);
			inView.AddRange(localInView);
		}

		foreach(Collider targetCol in inView)
		{
			Transform target = targetCol.transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;			
			if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
			{
				// Within my view angle
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				bool visionObstructed = false;
				foreach(LayerMask mask in obstacleMasks) 
				{
					visionObstructed = visionObstructed || Physics.Raycast(transform.position, dirToTarget, dstToTarget, mask);
				}
				if(!visionObstructed)
				{
					// I have you in my sight :eyes:
					this.Visible.Add(target.gameObject);
				}
			}
		}
	}

	IEnumerator FindTargetsWithDelay(float delay) 
	{
		while(true) 
		{
			yield return new WaitForSeconds(delay);
			GetVisibleTargetsHandler();
		}
	}

	void Start() 
	{
		StartCoroutine("FindTargetsWithDelay", this.delay);
	}
}