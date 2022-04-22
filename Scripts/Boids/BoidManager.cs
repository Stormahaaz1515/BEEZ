using System.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
	private static BoidManager instance = null;
	public static BoidManager sharedInstance
	{

		//Accesseur automatique
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<BoidManager>();
			}
			return instance;
		}
	}
	public Boid prefabBoid;
	public GameObject player;
	[Tooltip("Nombre d'agents")]
	public float nbBoids = 30;
	public float startSpeed = 1;
	public float startSpread = 10;
	public float timeBetweenHits = 1;

	private List<Boid> boids = new List<Boid>();
	public ReadOnlyCollection<Boid> roBoids
	{
		get { return new ReadOnlyCollection<Boid>(boids); }
	}

	private void Start()
	{
		for (int i = 0; i < nbBoids; i++)
		{
			Boid b = GameObject.Instantiate<Boid>(prefabBoid);
			Vector3 positionBoid = Random.insideUnitSphere * startSpread + transform.position;
			positionBoid.y = Mathf.Abs(positionBoid.y);
			b.transform.position = positionBoid;
			b.velocity = startSpeed * (positionBoid - transform.position).normalized;
			b.transform.parent = this.transform;
			b.managerAffected = this.gameObject.GetComponent<BoidManager>();
			b.player = player;
			boids.Add(b);
		}
	}
}