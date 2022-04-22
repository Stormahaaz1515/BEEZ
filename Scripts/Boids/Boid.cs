using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IL3DN;

public class Boid : MonoBehaviour
{
	public BoidManager managerAffected;
	public float zoneRepulsion = 5;
	public float zoneAlignement = 9;
	public float zoneAttraction = 50;

	public float forceRepulsion = 15;
	public float forceAlignement = 3;
	public float forceAttraction = 20;

	public Vector3 target = new Vector3();
	public float forceTarget = 20;
	public bool goToTarget = false;
	public bool joinTarget = false;
	public bool followTarget = false;
	public string gameObjectTarget = null;
	public GameObject player;
	public float catchTarget = 0;
	private bool tired = false;

	public Vector3 velocity = new Vector3();
	public float maxSpeed = 20;
	public float minSpeed = 12;
	public float adaptativeSpeed = 0;


	public bool drawGizmos = true;
	public bool drawLines = true;

	private float zoneAttractionSqr;
	private float zoneRepulsionSqr;
	private float zoneAlignementSqr;


	private void Start()
	{
		zoneAlignementSqr = zoneAlignement * zoneAlignement;
		zoneAttractionSqr = zoneAttraction * zoneAttraction;
		zoneRepulsionSqr = zoneRepulsion * zoneRepulsion;
	}
	// Update is called once per frame
	void Update()
	{
		Vector3 sumForces = new Vector3();
		Color colorDebugForce = Color.black;
		float nbForcesApplied = 0;

		foreach (Boid otherBoid in managerAffected.roBoids)
		{
			Vector3 vecToOtherBoid = otherBoid.transform.position - transform.position;

			Vector3 forceToApply = new Vector3();

			//Si on doit prendre en compte cet autre boid (plus grande zone de perception)
			if (vecToOtherBoid.sqrMagnitude < zoneAttractionSqr)
			{
				//Si on est entre attraction et alignement
				if (vecToOtherBoid.sqrMagnitude > zoneAlignementSqr)
				{
					//On est dans la zone d'attraction uniquement
					//forceToApply = vecToOtherBoid.normalized * forceAttraction; ça sert à quoi ? 
					float distToOtherBoid = vecToOtherBoid.magnitude;
					float normalizedDistanceToNextZone = ((distToOtherBoid - zoneAlignement) / (zoneAttraction - zoneAlignement));
					float boostForce = (4 * normalizedDistanceToNextZone);
					forceToApply = vecToOtherBoid.normalized * forceAttraction * boostForce;
					colorDebugForce += Color.green;
				}
				else
				{
					//On est dans alignement, mais est on hors de répulsion ?
					if (vecToOtherBoid.sqrMagnitude > zoneRepulsionSqr)
					{
						//On est dans la zone d'alignement uniquement
						forceToApply = otherBoid.velocity.normalized * forceAlignement;
						colorDebugForce += Color.blue;
					}
					else
					{
						//On est dans la zone de repulsion
						float distToOtherBoid = vecToOtherBoid.magnitude;
						float normalizedDistanceToPreviousZone = 1 - (distToOtherBoid / zoneRepulsion);
						float boostForce = (4 * normalizedDistanceToPreviousZone);
						forceToApply = vecToOtherBoid.normalized * -1 * (forceRepulsion * boostForce);
						colorDebugForce += Color.red;

					}
				}

				sumForces += forceToApply;
				nbForcesApplied++;
			}
		}

		//On fait la moyenne des forces, ce qui nous rend indépendant du nombre de boids
		sumForces /= nbForcesApplied;

		if ((transform.position - this.transform.parent.position).sqrMagnitude >= catchTarget * catchTarget * 3f)
			tired = true;

		//Si une cible à suivre est définie on récupère sa position, sinon la position du parent est attribuée
		if ((player.GetComponent<Transform>().position - transform.position).sqrMagnitude <= catchTarget * catchTarget && !tired)
		{
			target = player.GetComponent<Transform>().position;
			float distance = (target - transform.position).magnitude;
			if ((distance >= (catchTarget / 3) * 2))
			{
				adaptativeSpeed = Mathf.Lerp(player.GetComponent<SimpleFPSController>().m_RunSpeed * 1.5f, maxSpeed, 1f - (distance / ((catchTarget / 3) * 2)));
			}
			else
				adaptativeSpeed = Mathf.Lerp(maxSpeed, minSpeed, 1f - (distance / (catchTarget / 3)));
		}
		else
			target = this.transform.parent.position;


		//Si on a une target, on l'ajoute
		if (goToTarget)
		{
			Vector3 vecToTarget = target - transform.position;
			if (vecToTarget.sqrMagnitude < 1 && target == this.transform.parent.position)
				tired = false;
			if (goToTarget)
			{
				Vector3 forceToTarget = vecToTarget.normalized * forceTarget;
				sumForces += forceToTarget;
				colorDebugForce += Color.magenta;
				if (drawLines)
					Debug.DrawLine(transform.position, target, Color.magenta);
			}
		}


		//Debug
		if (drawLines)
			Debug.DrawLine(transform.position, transform.position + sumForces, colorDebugForce / nbForcesApplied);

		//On freine pour tourner
		velocity += -velocity * 10 * Vector3.Angle(sumForces, velocity) / 180.0f * Time.deltaTime;

		//on applique les forces

		//On limite la vitesse
		if (target == player.GetComponent<Transform>().position)
		{
			velocity += sumForces * Time.deltaTime;
			velocity = velocity.normalized * adaptativeSpeed;
		}
		else
		{
			velocity += sumForces * Time.deltaTime;
			if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
				velocity = velocity.normalized * maxSpeed;
			if (velocity.sqrMagnitude < minSpeed * minSpeed)
				velocity = velocity.normalized * minSpeed;
		}

		//On regarde dans la bonne direction        
		if (velocity.sqrMagnitude > 0)
			transform.LookAt(transform.position + velocity);

		//Debug
		if (drawLines)
			Debug.DrawLine(transform.position, transform.position + velocity, Color.blue);

		//Deplacement du boid
		transform.position += velocity * Time.deltaTime;
	}

	private void OnDrawGizmosSelected()
	{
		if (drawGizmos)
		{
			// Répulsion
			Gizmos.color = new Color(1, 0, 0, 1f);
			Gizmos.DrawWireSphere(transform.position, catchTarget);
			// Alignement 
			Gizmos.color = new Color(0, 1, 0, 1f);
			Gizmos.DrawWireSphere(transform.position, zoneAlignement);
			// Attraction
			Gizmos.color = new Color(0, 0, 1, 1f);
			Gizmos.DrawWireSphere(transform.position, zoneAttraction);
		}
	}
}
