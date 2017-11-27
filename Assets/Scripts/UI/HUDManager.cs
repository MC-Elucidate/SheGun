using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public Text healthText;
    public Text ammoText;

    private int health;

    private PlayerStatusManager playerStatus;
    private FireManager fireManager;

    void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStatus = player.GetComponent<PlayerStatusManager>();
        fireManager = player.GetComponent<FireManager>();
        health = playerStatus.Health;
	}
	
	void Update () {
        SetHealthText();
	}

    private void SetHealthText()
    {
        if (health != playerStatus.Health)
        {
            health = playerStatus.Health;
            healthText.text = "HP: " + health + "/" + playerStatus.MaxHealth;
        }
    }
}
