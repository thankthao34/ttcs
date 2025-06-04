using UnityEngine.UI;
using UnityEngine;
using Unity.Mathematics;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] public int currentCoin = 0;
    [SerializeField] private Text currentCointext;
    [SerializeField] private Text maxHealthText;
    [SerializeField] private Text Cointext;
    [SerializeField] private Text HealthText;
    [SerializeField] private Text enemiesKilledText;
    [Space(3)]
    [SerializeField] public int maxHealth = 10;

    [Header("Player Movement")]
    private float movement;
    [SerializeField] public float speed = 7f;
    [SerializeField] private float jumpHeight = 10f;

    private bool facingRight = true;
    private bool isGround = true;

    private Rigidbody2D rb;
    public Animator animator;

    [Header("Player Attack")]
    private Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;

    private bool isWon = false;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject floatingtextprefab;

    Text buttonText;

    // Biến thời gian rơi tự do dựa trên tọa độ y giảm
    private float fallTime = 0f;
    private const float MAX_FALL_TIME = 3f; // Thời gian tối đa y giảm trước khi thua
    private float previousY; // Tọa độ y trước đó

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        attackPoint = this.transform.GetChild(0).transform;
        previousY = transform.position.y; // Khởi tạo tọa độ y ban đầu

        if (GameManager.GM == null)
        {
            Debug.LogError("GameManager không tồn tại! Vui lòng thêm GameManager vào Scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isWon)
        {
            animator.SetFloat("Walk", 0);
            movement = 0f;
            speed = 0f;
            return;
        }
        if (maxHealth <= 0)
        {
            Die();
        }
        currentCointext.text = currentCoin.ToString();
        maxHealthText.text = maxHealth.ToString();
        Cointext.text = currentCoin.ToString();
        HealthText.text = maxHealth.ToString();
        movement = Input.GetAxis("Horizontal");

        movement = 0f;
        if (GameManager.GM != null)
        {
            if (Input.GetKey(GameManager.GM.left))  
            {
                movement = -1f;
            }
            else if (Input.GetKey(GameManager.GM.right))  
            {
                movement = 1f;
            }
        }

        if (movement < 0f && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        if (GameManager.GM != null && Input.GetKeyDown(GameManager.GM.jump) && isGround == true)
        {
            Jump();
            animator.SetBool("Jump", true);
            isGround = false;
        }

        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Walk", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Walk", 0f);
        }

        if (GameManager.GM != null && Input.GetKeyDown(GameManager.GM.attack))
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            if (randomIndex == 0)
            {
                animator.SetTrigger("Attack1");
            }
            else if (randomIndex == 1)
            {
                animator.SetTrigger("Attack2");
            }
            else
            {
                animator.SetTrigger("Attack3");
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * speed;

        // Kiểm tra tọa độ y giảm liên tục
        float currentY = transform.position.y;
        if (currentY < previousY) 
        {
            fallTime += Time.fixedDeltaTime;
            Debug.Log("Falling time: " + fallTime + "s, Current Y: " + currentY + ", Previous Y: " + previousY);
            if (fallTime >= MAX_FALL_TIME)
            {
                Debug.Log("Player thua do tọa độ y giảm quá 3s!");
                maxHealth = 0; 
            }
        }
        else
        {
            fallTime = 0f; // Reset khi tọa độ y không giảm
        }
        previousY = currentY; // Cập nhật tọa độ y trước đó
    }

    void Jump()
    {
        Vector2 velocity = rb.velocity;
        velocity.y = jumpHeight;
        rb.velocity = velocity;
        FindObjectOfType<Sound>().PlayJumpSound();
    }

    public void PlayerAttack()
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo)
        {
            if (hitInfo.GetComponent<Enemy1>() != null)
            {
                hitInfo.GetComponent<Enemy1>().EnemyTakeDamge(1);
                GameObject tempfloatingText = Instantiate(floatingtextprefab, hitInfo.transform.position, Quaternion.identity);
                Destroy(tempfloatingText, 1.1f);
            }

            if (hitInfo.GetComponent<minionPrefab>() != null)
            {
                hitInfo.GetComponent<minionPrefab>().MinionsTakeDamge(1);
                GameObject tempfloatingText = Instantiate(floatingtextprefab, hitInfo.transform.position, Quaternion.identity);
                Destroy(tempfloatingText, 1.1f);
            }

            if (hitInfo.GetComponent<Undead_Boss>() != null)
            {
                hitInfo.GetComponent<Undead_Boss>().BossTakeDamge(1);
                GameObject tempfloatingText = Instantiate(floatingtextprefab, hitInfo.transform.position, Quaternion.identity);
                Destroy(tempfloatingText, 1.1f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
            Debug.Log("Chạm đất, isGround = true");
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        CameraShake.instance.Shake(3f, .2f);
        maxHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("collect");
            Destroy(other.gameObject, 0f);
        }

        if (other.gameObject.tag == "Trap")
        {
            Die();
        }

        if (other.gameObject.tag == "Key")
        {
            victoryUI.SetActive(true);
            isWon = true;
            Destroy(other.gameObject, 0f);
        }

        if (other.gameObject.tag == "Heart")
        {
            maxHealth++;
            Destroy(other.gameObject, 0f);
            Debug.Log("Va chạm với: " + other.gameObject.name);
        }

        if (enemiesKilledText != null && GameManager.GM != null)
        {
            enemiesKilledText.text = GameManager.GM.enemiesKilled.ToString();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    void Die()
    {
        Debug.Log(this.transform.name + " Died");
        gameOverUI.SetActive(true);
        CameraShake.instance.Shake(5f, .3f);
        GameObject temp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(temp, .9f);
        Destroy(this.gameObject);
    }
}