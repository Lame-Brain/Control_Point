using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CP_Controller : MonoBehaviour
{
    private float friendlyCapture, foeCapture;
    private bool awardedPoints;
    public string owner;
    public GameObject gTrack, oTrack, wTrack;

    // Start is called before the first frame update
    void Start()
    {
        friendlyCapture = 0;
        foeCapture = 0;
        owner = "unclaimed";
        awardedPoints = false;
        wTrack.SetActive(true); gTrack.SetActive(false); oTrack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (friendlyCapture < 0) friendlyCapture = 0; if (friendlyCapture > 110) friendlyCapture = 110;
        if (foeCapture < 0) foeCapture = 0; if (foeCapture > 110) foeCapture = 110;
        if (friendlyCapture >= 100 && foeCapture < 99) { GetComponent<SpriteRenderer>().color = Color.green; owner = "Player"; }
        if (foeCapture >= 100 && friendlyCapture < 99) { GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f); owner = "Aliens"; }
        if (owner == "unclaimed" && !wTrack.activeInHierarchy) { wTrack.SetActive(true); gTrack.SetActive(false); oTrack.SetActive(false); }
        if (owner == "Player" && !gTrack.activeInHierarchy) { wTrack.SetActive(false); gTrack.SetActive(true); oTrack.SetActive(false); }
        if (owner == "Aliens" && !oTrack.activeInHierarchy) { wTrack.SetActive(false); gTrack.SetActive(false); oTrack.SetActive(true); }
        if (owner == "Player" && !awardedPoints) { GameManager.POINTS += 10000; awardedPoints = true; }
        if (owner == "Aliens" && awardedPoints) { GameManager.POINTS -= 10000; awardedPoints = false; }
    }

    public void OnTriggerStay(Collider col)    {
        if (col.tag == "Player")
        {
            GameManager.GAME.gainCP_UI.gameObject.SetActive(true);
            GameManager.GAME.friend_UI.gameObject.SetActive(true);
            GameManager.GAME.friend_UI.text = friendlyCapture.ToString("F1") + "%";
            GameManager.GAME.foe_UI.gameObject.SetActive(true);
            GameManager.GAME.foe_UI.text = foeCapture.ToString("F1") + "%";

            
            friendlyCapture += .5f;
            foeCapture -= 0.1f;
        }
        if (col.tag == "Enemy")
        {
            GameManager.GAME.loseCP_UI.gameObject.SetActive(true);
            friendlyCapture -= 0.1f;
            foeCapture += 0.1f;
        }
    }
    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            GameManager.GAME.gainCP_UI.gameObject.SetActive(false);
            GameManager.GAME.friend_UI.gameObject.SetActive(false);
            GameManager.GAME.foe_UI.gameObject.SetActive(false);
        }
        if (col.tag == "Enemy") GameManager.GAME.loseCP_UI.gameObject.SetActive(false);
    }
}
