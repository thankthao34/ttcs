using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead_Boss : MonoBehaviour
{
    public Animator animator;
    public int maxHealth =10;
    public Transform player;
    public float attackRange=10f;
    private bool playerInRange = false;
    public float runSpeed = 2.5f;
    public float chaseSpeed = 5f;
    public float retrieveDistance =2.5f;

    public Transform detectPoint;
    public float distance;
    public LayerMask detecLayer;
    private bool facingLeft = true;

    public Transform attackPoint;
    public float attackRadius = 3.5f;
    public LayerMask attackLayer;

    // minions
    public GameObject minionPrefab;
    public Transform spawnPoint;
    public int maxMinions = 3;
    private List<GameObject> spawnedMinions = new List<GameObject>();


    void Update()
    {
        if( maxHealth <=0){
            Die();
        }

        if(player == null){
            animator.SetBool("PlayerDead",true);
            return;
        }
        
        if(Vector2.Distance(transform.position, player.position) <= attackRange){
            playerInRange = true;
        }
        else{
            playerInRange = false;
        }
        
        if(playerInRange){
            if(transform.position.x <player.position.x && facingLeft){
                transform.eulerAngles = new Vector3(0f,-180,0f);
                facingLeft = false;
            }
            else if (transform.position.x > player.position.x && facingLeft == false){
                transform.eulerAngles = new Vector3(0f,0f,0f);
                facingLeft = true;
            }
            if(Vector2.Distance(transform.position, player.position) > retrieveDistance){
                animator.SetBool("Attack",false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

            }
            else{
                animator.SetBool("Attack",true);
            }
        }
    }

    public void Attack(){
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius,attackLayer);
        if(collInfo){
            if(collInfo.GetComponent<Player>() != null){
                collInfo.GetComponent<Player>().PlayerTakeDamage(1);
                
            }
            Debug.Log(collInfo.gameObject.name+"takes damage");
        }
    }

    public void BossTakeDamge(int damage){
        if(maxHealth <=0){
            return;
        }
        CameraShake.instance.Shake(3f,.2f);
        maxHealth -= damage;
        animator.SetTrigger("Damage");
        TrySpawnMinions();
    }

    void TrySpawnMinions(){
        spawnedMinions.RemoveAll(minion => minion == null);

        int currentCount = spawnedMinions.Count;
        int spawnCount = Mathf.Min(1, maxMinions - currentCount);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            GameObject minion = Instantiate(minionPrefab, spawnPoint.position + offset, Quaternion.identity);

            // Reset Animator để đảm bảo appear animation chạy từ đầu
            Animator ani = minion.GetComponent<Animator>();
            if (ani != null)
            {
                ani.Play("Summon_Appear", -1, 0f); // Phát lại appear ngay từ đầu
            }

            spawnedMinions.Add(minion);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null){
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position,Vector2.down * distance);

        if(attackPoint != null){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
    void Die() {
        Debug.Log(this.gameObject.name + " Died");
        CameraShake.instance.Shake(5f,.3f);
        // GameObject temp = Instantiate(explosionPrefab,feetPoint.position, Quaternion.identity);
        // Destroy(temp,.9f);
        Destroy(this.gameObject);
    
    }
}
