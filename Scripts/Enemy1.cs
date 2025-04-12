using UnityEngine;

public class Enemy1 : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

            if(Vector2.Distance(transform.position, player.position) > retrieveDistance){
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

            }
            else{
                Debug.Log("Attack");
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
    private void ODrawGizmosSelected()
    {
        if (detectPoint == null){
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position,Vector2.down * distance);
    }
}
