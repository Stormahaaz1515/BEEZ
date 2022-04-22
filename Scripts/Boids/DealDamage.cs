using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
	private GameObject player;
	public float damageRange = 3;
	public int damageAmount = 7;
	public float timeBetweenHits = 0;
	private float delayBeforeHit;
	private void Start()
	{
		player = this.gameObject.GetComponent<Boid>().player;
	}
	// Update is called once per frame
	void Update()
	{
		if (player != null)
			if ((player.GetComponent<Transform>().position - transform.position).sqrMagnitude <= damageRange * damageRange)
				player.GetComponent<HealthPlayer>().TakeDamage(damageAmount);
	}
}
