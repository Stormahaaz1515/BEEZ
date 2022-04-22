using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidFlying : MonoBehaviour
{
	public BoidFlyingManager managerAffected;
	public float zoneRepulsion = 5;
	public float zoneAlignement = 9;
	public float zoneAttraction = 50;

	public float forceRepulsion = 15;
	public float forceAlignement = 3;
	public float forceAttraction = 20;

	public Transform target;
	public float forceTarget = 20;
	public bool goToTarget = false;

	public Vector3 velocity = new Vector3();
	public float maxSpeed = 20;
	public float minSpeed = 12;


	public bool drawGizmos = true;
	public bool drawLines = true;

	// Update is called once per frame
	void Update()
	{
		Vector3 sumForces = new Vector3();
		Color colorDebugForce = Color.black;
		float nbForcesApplied = 0;

		foreach (BoidFlying otherBoid in managerAffected.FlyingBoids)
		{
			Vector3 vecToOtherBoid = otherBoid.transform.position - transform.position;

			Vector3 forceToApply = new Vector3();

			//Si on doit prendre en compte cet autre boid (plus grande zone de perception)
			if (vecToOtherBoid.sqrMagnitude < zoneAttraction * zoneAttraction)
			{
				//Si on est entre attraction et alignement
				if (vecToOtherBoid.sqrMagnitude > zoneAlignement * zoneAlignement)
				{
					//On est dans la zone d'attraction uniquement
					//forceToApply = vecToOtherBoid.normalized * forceAttraction; �a sert � quoi ? 
					float distToOtherBoid = vecToOtherBoid.magnitude;
					float normalizedDistanceToNextZone = ((distToOtherBoid - zoneAlignement) / (zoneAttraction - zoneAlignement));
					float boostForce = (4 * normalizedDistanceToNextZone);
					forceToApply = vecToOtherBoid.normalized * forceAttraction * boostForce;
					colorDebugForce += Color.green;
				}
				else
				{
					//On est dans alignement, mais est on hors de r�pulsion ?
					if (vecToOtherBoid.sqrMagnitude > zoneRepulsion * zoneRepulsion)
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

		//On fait la moyenne des forces, ce qui nous rend ind�pendant du nombre de boids
		sumForces /= nbForcesApplied;

		//Si on a une target, on l'ajoute
		if (goToTarget)
		{
			Vector3 vecToTarget = target.position - transform.position;
			Vector3 forceToTarget = vecToTarget.normalized * forceTarget;
			sumForces += forceToTarget;
			colorDebugForce += Color.magenta;
			nbForcesApplied++;
			if (drawLines)
				Debug.DrawLine(transform.position, target.position, Color.magenta);
		}

		//Debug
		if (drawLines)
			Debug.DrawLine(transform.position, transform.position + sumForces, colorDebugForce / nbForcesApplied);

		//On freine
		velocity += -velocity * 10 * Vector3.Angle(sumForces, velocity) / 180.0f * Time.deltaTime;

		//on applique les forces
		velocity += sumForces * Time.deltaTime;

		//On limite la vitesse
		if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
			velocity = velocity.normalized * maxSpeed;
		if (velocity.sqrMagnitude < minSpeed * minSpeed)
			velocity = velocity.normalized * minSpeed;

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
			// R�pulsion
			Gizmos.color = new Color(1, 0, 0, 1f);
			Gizmos.DrawWireSphere(transform.position, zoneRepulsion);
			// Alignement 
			Gizmos.color = new Color(0, 1, 0, 1f);
			Gizmos.DrawWireSphere(transform.position, zoneAlignement);
			// Attraction
			Gizmos.color = new Color(0, 0, 1, 1f);
			Gizmos.DrawWireSphere(transform.position, zoneAttraction);
		}
	}
}
