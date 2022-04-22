using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public GameObject player;
    public bool atHome = false;
    public bool pause = false;
    [SerializeField] private GameObject gameMenuUi;
    [SerializeField] private GameObject winMenuUi;
    [SerializeField] private GameObject instructionsUi;


    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<HealthPlayer>().playerIsDead)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (player.GetComponent<HealthPlayer>().invicible == false)
            if (GameObject.FindGameObjectWithTag("Finish").GetComponent<IsOnGoal>().isOnGoal != false)
            {
                player.GetComponent<HealthPlayer>().invicible = true;
                winMenuUi.SetActive(true);
            }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
            {
                pause = true;
                Pause();
            }
            else
            {
                pause = false;
                Resume();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gameMenuUi.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameMenuUi.SetActive(false);
        instructionsUi.SetActive(false);
    }

}