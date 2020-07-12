using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    public AudioSource[] Line;
    public GameObject[] LineText;
    private int story = 0;
    void Start()
    {
        TellStory();
        
    }
    void Update()
    {
        if (Input.anyKey || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            SceneManager.LoadScene("Game");
        }
        
    }

    public void TellStory()
    {
        float wait = Line[story].clip.length;
        Debug.Log("Clip #" + story + 1 + " is " + wait + " units long");
        Line[story].Play();
        LineText[story].SetActive(true);
        story++;
        if (story < 9) Invoke("TellStory", wait+0.5f);
        //if(Line[story].time == wait) Invoke("TellStory", 2);
    }
}
