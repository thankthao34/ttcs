using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int maxHealth =3;
    public Animator animator;
    public Transform player;
    public float attackRange=10f;
    private bool playerInRange = false;
    public float runSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float retrieveDistance =2.5f;

    public Transform detectPoint;
    public float distance;
    public LayerMask detecLayer;
    private bool facingLeft = true;

    public Transform attackPoint;
    public float attackRadius = 2f;
    public LayerMask attackLayer;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform feetPoint;

    public GameObject heartPrefab;
    public GameObject coinPrefab;

    

    // Update is called once per frame
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
        if(playerInRange == true){

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

    public void Attack(){
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius,attackLayer);
        if(collInfo){
            if(collInfo.GetComponent<Player>() != null){
                collInfo.GetComponent<Player>().PlayerTakeDamage(1);
            }
        }
    }

    public void EnemyTakeDamge(int damage){
        if(maxHealth <=0){
            return;
        }
        CameraShake.instance.Shake(3f,.2f);
        maxHealth -= damage;
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
    void Die()
    {
        Debug.Log(this.gameObject.name + " Died");
        CameraShake.instance.Shake(5f, .3f);
        GameObject temp = Instantiate(explosionPrefab, feetPoint.position, Quaternion.identity);
        Destroy(temp, .9f);
        if (GameManager.GM != null)
        {
            GameManager.GM.AddEnemyKilled();
        }
        Destroy(this.gameObject);
        GameObject prefabToSpawn = (UnityEngine.Random.value > 0.5f) ? heartPrefab : coinPrefab;
        if (prefabToSpawn != null)
        {
            GameObject spawnedItem = Instantiate(prefabToSpawn, feetPoint.position, Quaternion.identity);
            // Tự hủy vật phẩm sau 3 giây
            Destroy(spawnedItem, 3f);
        }
        
        }
}
