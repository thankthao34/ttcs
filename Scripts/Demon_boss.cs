using UnityEngine;
public class Demon_boss : MonoBehaviour

{
    public Animator animator;
    private bool playerInRange = false;
    public Transform player;
    public float attackRange=15f;
    public float runSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float retrieveDistance =2.5f;
    public Transform detectPoint;
    public float distance;
    public LayerMask detecLayer;
    private bool facingLeft = true;

    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) <= attackRange){
            playerInRange = true;
        }
        else{
            playerInRange = false;
        }

        if(playerInRange == true){

            if(transform.position.x <player.position.x && facingLeft){
                transform.eulerAngles = new Vector3(0f,-180,0f);
                facingLeft = false;
            }
            else if (transform.position.x > player.position.x && facingLeft == false){
                transform.eulerAngles = new Vector3(0f,0f,0f);
                facingLeft = true;
            }

            if(Vector2.Distance(transform.position, player.position) <= retrieveDistance){
                animator.SetBool("Attack",false);
                Debug.Log("k kích hoạt Attack animation");
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

            }
            else{
                Debug.Log("Đang cố gắng kích hoạt Attack animation");
                animator.SetBool("Attack", true);
            }
        }
        else{
            transform.Translate(Vector2.left * runSpeed * Time.deltaTime );

            RaycastHit2D hit = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, detecLayer);

            if (hit == false){
                if( facingLeft == true){
                    transform.eulerAngles = new Vector3(0,-180,0);
                    facingLeft = false;

                }
                else if(facingLeft == false){
                    transform.eulerAngles = new Vector3(0,0,0);
                    facingLeft = true;

                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null){
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position,Vector2.down * distance);
        // if(attackPoint != null){
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        // }
    }
}