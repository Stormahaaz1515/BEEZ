using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuControl : MonoBehaviour
{
	public void PlayGame()
	{
		SceneManager.LoadSceneAsync("The Game");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public AudioMixer audioMixer;
	public void SetVolume(float volumeParam)
	{
		audioMixer.SetFloat("volumeParam", volumeParam);
	}
}
