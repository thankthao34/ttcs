
using UnityEngine;

public class Undead_Die : MonoBehaviour
{
    public Transform player;
    private bool facingLeft = true;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < player.position.x && facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (transform.position.x > player.position.x && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }
        
    }
}
