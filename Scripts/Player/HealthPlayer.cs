using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
	public int maxHealth = 100;
	public int currentHealth;
	public bool playerIsDead = false;
	public bool hitable = false;
	public float timeBetweenHits = 1f;
	private float delayBeforeHit;
	public bool invicible = false;
	private AudioSource audioSource;
	[SerializeField] private AudioClip hit;

	public HealthBar heaalthBar;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		delayBeforeHit = timeBetweenHits;
		currentHealth = maxHealth;
		heaalthBar.SetMaxHealth(maxHealth);
	}

	private void Update()
	{
		if (currentHealth <= 0)
			playerIsDead = true;
	}

	public void TakeDamage(int damage)
	{
		delayBeforeHit -= Time.deltaTime;
		if (delayBeforeHit <= 0)
			if (!invicible)
				hitable = true;
		if (hitable)
		{
			currentHealth -= damage;
			audioSource.clip = hit;
			audioSource.PlayOneShot(audioSource.clip);
			heaalthBar.SetHealth(currentHealth);
			hitable = false;
			delayBeforeHit = timeBetweenHits;
		}
	}
}