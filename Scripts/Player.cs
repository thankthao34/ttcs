using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movement;
    public float speed = 7f;
    public float jumpHeight = 10f;

    private bool facingRight = true;
    private bool isGround = true;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            isGround = false;  
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground"){
            isGround = true;
        }
    }
}
