using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuInGame : MonoBehaviour
{
	[SerializeField] private GameObject player;
	public GameHandler gameHandler;

	public void Menu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void Resume()
	{
		gameHandler.GetComponent<GameHandler>().Resume();
	}
	public void QuitGame()
	{
		Application.Quit();
	}
	
	public void SetInvincible(float invincible)
	{
		if (invincible == 0)
			player.GetComponent<HealthPlayer>().invicible = false;
		else
			player.GetComponent<HealthPlayer>().invicible = true;
	}

	public AudioMixer audioMixer;
	public void SetVolume(float volumeParam)
	{
		audioMixer.SetFloat("volumeParam", volumeParam);
	}
}
