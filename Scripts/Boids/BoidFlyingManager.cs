using System.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoidFlyingManager : MonoBehaviour
{
	private static BoidFlyingManager instance = null;
	public static BoidFlyingManager sharedInstance
	{

		//Accesseur automatique
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<BoidFlyingManager>();
			}
			return instance;
		}
	}
	public BoidFlying prefabBoid;
	[Tooltip("Nombre d'agents")]
	public float nbBoids = 30;
	public float startSpeed = 1;
	public float startSpread = 10;
	public Transform target;
	public float forceTarget;
	
	private List<BoidFlying> boids = new List<BoidFlying>();
	public ReadOnlyCollection<BoidFlying> FlyingBoids
	{
		get { return new ReadOnlyCollection<BoidFlying>(boids); }
	}

	private void Start()
	{
		for (int i = 0; i < nbBoids; i++)
		{
			BoidFlying b = GameObject.Instantiate<BoidFlying>(prefabBoid);
			Vector3 positionBoid = Random.insideUnitSphere * startSpread + transform.position;
			positionBoid.y = Mathf.Abs(positionBoid.y);
			b.transform.position = positionBoid;
			b.velocity = startSpeed * (positionBoid - transform.position).normalized;
			b.transform.parent = this.transform;
			b.managerAffected = this.gameObject.GetComponent<BoidFlyingManager>();
			if (target)
			{
				b.goToTarget = true;
				b.target = target;
				b.forceTarget = forceTarget;
			}
			boids.Add(b);
		}
	}
}