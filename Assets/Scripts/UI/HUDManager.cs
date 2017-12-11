using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public Slider healthSlider;
    public Slider energySlider;

    private int health;
    private float energy;

    private PlayerStatusManager playerStatus;
    private GunDatsuManager gunDatsuManager;

    void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStatus = player.GetComponent<PlayerStatusManager>();
        gunDatsuManager = player.GetComponent<GunDatsuManager>();
        health = playerStatus.Health;
        energy = gunDatsuManager.energyMax;
        energySlider.maxValue = energy;
        healthSlider.maxValue = playerStatus.MaxHealth;
	}
	
	void Update () {
        SetHealthText();
        SetEnergy();
	}

    private void SetHealthText()
    {
        if (health != playerStatus.Health)
        {
            health = playerStatus.Health;
            healthSlider.value = health;
        }
    }

    private void SetEnergy()
    {
        if (energy != gunDatsuManager.energyCurrent)
        {
            energy = gunDatsuManager.energyCurrent;
            energySlider.value = energy;
        }
    }
}
