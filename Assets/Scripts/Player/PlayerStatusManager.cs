using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour {


    [SerializeField]
    public int MaxHealth = 10;
    [ReadOnly]
    [SerializeField]
    public int Health;

    [ReadOnly]
    public EPlayerState playerState;

    private Vector2 respawnLocation;
    private SpriteRenderer sprite;
    private MovementManager movementManager;
    private Color defaultColour;
    private Color damageFlashColour;

    void Start () {
        Health = MaxHealth;
        respawnLocation = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        movementManager = GetComponent<MovementManager>();
        defaultColour = sprite.color;
        damageFlashColour = Color.red;
        playerState = EPlayerState.FreeMovement;
    }
	
	void Update () {
		
	}

    public void ReceiveDamage(int damage, Transform sourceOfDamage)
    {
        if (playerState == EPlayerState.Executing)
            return;

        Health -= damage;
        if (Health <= 0)
        {
            Respawn();
            return;
        }
        EDirection directionOfContact = sourceOfDamage.position.x >= transform.position.x ? EDirection.DownRight : EDirection.DownLeft;
        movementManager.KickBack(directionOfContact);
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
