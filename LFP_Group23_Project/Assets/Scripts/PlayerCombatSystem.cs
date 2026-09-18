using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsProjectiles;
    public float attackRange;
    public int damage;

    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies, whatIsProjectiles);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }

            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
       Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

    }

}
