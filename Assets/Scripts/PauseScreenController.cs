using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenController : MonoBehaviour
{
    public GameObject restartSubPanel, exitSubPanel, creditsSubPanel, helpSubPanel;

    // Update is called once per frame
    void Update()
    {
        if (!restartSubPanel.activeInHierarchy && !exitSubPanel.activeInHierarchy && !creditsSubPanel.activeInHierarchy && !helpSubPanel.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GameManager.GAME.PauseGameToggle();
            }
            if (Input.GetKeyUp(KeyCode.H))
            {
                helpSubPanel.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                creditsSubPanel.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                restartSubPanel.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                exitSubPanel.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                restartSubPanel.SetActive(false);
                exitSubPanel.SetActive(false);
                helpSubPanel.SetActive(false);
                creditsSubPanel.SetActive(false);
            }
        }
    }
}
