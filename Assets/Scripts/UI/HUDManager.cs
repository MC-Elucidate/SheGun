using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public Slider healthSlider;
    public Slider energySlider;

    private RectTransform healthTransform;

    private int health;
    private int maxHealth;
    private float energy;

    private PlayerStatusManager playerStatus;
    private GunDatsuManager gunDatsuManager;

    private float healthSliderSizeMultiplier;

    void Start () {
        healthTransform = healthSlider.gameObject.GetComponent<RectTransform>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStatus = player.GetComponent<PlayerStatusManager>();
        gunDatsuManager = player.GetComponent<GunDatsuManager>();
        health = playerStatus.Health;
        maxHealth = playerStatus.MaxHealth;
        energy = playerStatus.Energy;
        energySlider.maxValue = energy;
        healthSlider.maxValue = playerStatus.MaxHealth;
        healthSliderSizeMultiplier = healthTransform.sizeDelta.x / maxHealth;
	}
	
	void Update () {
        SetMaxHealth();
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
        if (energy != playerStatus.Energy)
        {
            energy = playerStatus.Energy;
            energySlider.value = energy;
        }
    }

    private void SetMaxHealth()
    {
        if (maxHealth != playerStatus.MaxHealth)
        {
            float healthIncreaseAmount = playerStatus.MaxHealth - maxHealth;
            maxHealth = playerStatus.MaxHealth;
            healthSlider.maxValue = maxHealth;
            healthTransform.sizeDelta += new Vector2(healthIncreaseAmount * healthSliderSizeMultiplier, 0);
        }
    }
}
