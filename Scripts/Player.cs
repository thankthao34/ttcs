using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int maxHealth =10 ;
    public float movement;
    public float speed = 7f;
    public float jumpHeight = 10f;

    private bool facingRight = true;
    private bool isGround = true;
    public Rigidbody2D rb;
    public Animator animator;

    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(maxHealth <=0){
            Die();
        }
        movement = Input.GetAxis("Horizontal");

        if(movement <0f && facingRight == true ){
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if(movement >0f && facingRight == false){
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
        if(Input.GetKey(KeyCode.Space) && isGround == true){
            Jump();
            animator.SetBool("Jump", true);
            isGround = false;  
        }

        if(Mathf.Abs(movement) > .1f) {
            animator.SetFloat("Walk", 1f);
        }
        else if(movement <.1f) {
            animator.SetFloat("Walk", 0f);
        }
        
        if(Input.GetMouseButtonDown(0)){
            int randomIndex = Random.Range(0,3);
            if(randomIndex == 0){
                animator.SetTrigger("Attack1");
            }
            else if (randomIndex == 1){
                animator.SetTrigger("Attack2");
            }
            else 
                animator.SetTrigger("Attack3");
        }
    }

    private void FixedUpdate(){
        transform.position += new Vector3(movement,0f,0f) * Time.fixedDeltaTime * speed;
    }
    void Jump(){
        Vector2 velocity = rb.velocity ;
        velocity.y = jumpHeight;
        rb.velocity = velocity;
    }

    public void PlayerAttack(){
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if ( hitInfo){
            if(hitInfo.GetComponent<Enemy1>()!= null){
                hitInfo.GetComponent<Enemy1>().EnemyTakeDamge(1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground"){
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }
    public void PlayerTakeDamage( int damage){
        if( maxHealth <=0){
            return;
        }
        maxHealth -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null){
            return ;

        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);

    }

    void Die(){
        Debug.Log(this.transform.name + " Died");
        Destroy(this.gameObject);
    }
}
