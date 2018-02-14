using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour {


    [SerializeField]
    public int MaxHealth = 10;
    [ReadOnly]
    [SerializeField]
    public int Health;
    [SerializeField]
    public float MaxEnergy = 100;
    [ReadOnly]
    [SerializeField]
    public float Energy;

    [ReadOnly]
    public EPlayerState playerState;

    private Vector2 respawnLocation;
    private SpriteRenderer sprite;
    private MovementManager movementManager;
    private MeleeManager meleeManager;
    private Color defaultColour;
    private Color damageFlashColour;

    void Start () {
        Health = MaxHealth;
        Energy = MaxEnergy;
        respawnLocation = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        movementManager = GetComponent<MovementManager>();
        meleeManager = GetComponent<MeleeManager>();
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
        movementManager.DashReleased();
        meleeManager.EndAttack();
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

    public void UpgradeHealth()
    {
        MaxHealth += 2;
        Health = MaxHealth;
    }

    public void OutOfBounds(Vector3 recoveryPosition)
    {
        if (Health > 1)
        {
            transform.position = recoveryPosition;
        }
        ReceiveDamage(1, transform);
    }

    public void RegenerateOnExecute(EnemyStatusManager enemy)
    {
        RegenHealth(enemy.executeHealthDrop);
        RegenEnergy(enemy.executeEnergyDrop);
    }

    private void RegenHealth(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    private void RegenEnergy(int amount)
    {
        Energy += amount;
        if (Energy > MaxEnergy)
            Energy = MaxEnergy;
    }
}
