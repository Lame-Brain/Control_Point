using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenController : MonoBehaviour
{
    public GameObject creditsScreen_UI, helpScreen_UI, quitConfirmScreenUI, MRCTRL;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", .5f * Time.time);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!creditsScreen_UI.activeInHierarchy && !helpScreen_UI.activeInHierarchy && !quitConfirmScreenUI.activeInHierarchy)
            {
                quitConfirmScreenUI.SetActive(true);
            }
            else
            {
                creditsScreen_UI.SetActive(false);
                helpScreen_UI.SetActive(false);
                quitConfirmScreenUI.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.P) && !creditsScreen_UI.activeInHierarchy && !helpScreen_UI.activeInHierarchy && !quitConfirmScreenUI.activeInHierarchy) StartGame();
        if (Input.GetKeyDown(KeyCode.H) && !creditsScreen_UI.activeInHierarchy && !helpScreen_UI.activeInHierarchy && !quitConfirmScreenUI.activeInHierarchy) helpScreen_UI.SetActive(true);
        if (Input.GetKeyDown(KeyCode.C) && !creditsScreen_UI.activeInHierarchy && !helpScreen_UI.activeInHierarchy && !quitConfirmScreenUI.activeInHierarchy) creditsScreen_UI.SetActive(true);       
        if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)) MRCTRL.SetActive(true);
        if (Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.LeftControl)) MRCTRL.SetActive(false);

        if (quitConfirmScreenUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Y)) QuitGame();
            if (Input.GetKeyDown(KeyCode.N)) quitConfirmScreenUI.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Story");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
