using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour {


    [SerializeField]
    private int MaxHealth = 10;
    [SerializeField]
    private int Health;

    private Vector2 respawnLocation;
    protected SpriteRenderer sprite;
    private Color defaultColour;
    private Color damageFlashColour;

    void Start () {
        Health = MaxHealth;
        respawnLocation = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        defaultColour = sprite.color;
        damageFlashColour = Color.red;
    }
	
	void Update () {
		
	}

    public void ReceiveDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            Respawn();
        StartCoroutine(DamageFlash());
    }

    private void Respawn()
    {
        Health = MaxHealth;
        transform.position = respawnLocation;
    }

    protected IEnumerator DamageFlash()
    {
        sprite.color = damageFlashColour;
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.color = defaultColour;
    }
}
