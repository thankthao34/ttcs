
using UnityEngine;

public class minionPrefab : MonoBehaviour
{
    public Transform player;
    public float attackRange=10f;
    private bool playerInRange = false;
    public Animator animator;
    
    public float runSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float retrieveDistance =2.5f;

    public Transform detectPoint;
    public float distance;
    
    public Transform attackPoint;
    public float attackRadius = 3.5f;
    public LayerMask attackLayer;

    public int maxHealth =2;

    

    // Update is called once per frame
    void Update()
    {
        if( maxHealth <=0 ){
            Die();
        }

        if(player == null ){
            animator.SetBool("PlayerDead",true);
            return;
        }

        if(Vector2.Distance(transform.position, player.position) <= attackRange){
            playerInRange = true;
        }
        else{
            playerInRange = false;
        }

        if(playerInRange == true){
            if(Vector2.Distance(transform.position, player.position) > retrieveDistance){
                animator.SetBool("Attack",false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

            }
            else{
                animator.SetBool("Attack", true);
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

    public void MinionsTakeDamge(int damage){
        if(maxHealth <=0){
            return;
        }
        CameraShake.instance.Shake(3f,.2f);
        maxHealth -= damage;
    }

    void Die() {
        Debug.Log(this.gameObject.name + " Died");
        Destroy(this.gameObject);
    }
}
