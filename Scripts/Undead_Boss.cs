using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Undead_Boss : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 10;
    public Transform player;
    public float attackRange = 10f;
    private bool playerInRange = false;
    public float runSpeed = 2.5f;
    public float chaseSpeed = 5f;
    public float retrieveDistance = 2.5f;

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

    public AnimationClip dieClip;

    void Update()
    {
        if (maxHealth <= 0 )
        {
            Die();
        }

        if (player == null)
        {
            animator.SetBool("PlayerDead", true);
            return;
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange)
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
            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack", true);
            }
        }
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.GetComponent<Player>() != null)
            {
                collInfo.GetComponent<Player>().PlayerTakeDamage(1);
            }
            Debug.Log(collInfo.gameObject.name + "takes damage");
        }
    }

    public void BossTakeDamge(int damage)
    {
        if (maxHealth <= 0 )
        {
            return;
        }
        CameraShake.instance.Shake(3f, 0.2f);
        maxHealth -= damage;
        animator.SetTrigger("Damage");
        TrySpawnMinions();
    }

    void TrySpawnMinions()
    {
        int currentCount = spawnedMinions.Count;
        int spawnCount = Mathf.Min(1, maxMinions - currentCount);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            GameObject minionObject = Instantiate(minionPrefab, spawnPoint.position + offset, Quaternion.identity);
            minionPrefab minionScript = minionObject.GetComponent<minionPrefab>();

            if (minionScript != null && player != null)
            {
                minionScript.player = player;
            }

            Animator ani = minionObject.GetComponent<Animator>();
            if (ani != null)
            {
                ani.Play("Summon_Appear", -1, 0f);
            }

            spawnedMinions.Add(minionObject);
        }
    }

void Die() {
    Debug.Log(this.gameObject.name + " Died");
    CameraShake.instance.Shake(5f, .3f);
    animator.SetTrigger("Die");
    
    // Xóa tất cả minions
    foreach (GameObject minion in spawnedMinions)
    {
        if (minion != null)
        {
            Destroy(minion);
        }
    }
    spawnedMinions.Clear();
    
    // Sử dụng coroutine để đợi animation kết thúc
    StartCoroutine(DestroyAfterAnimation());
}

IEnumerator DestroyAfterAnimation()
{
    // Đợi một frame để animator bắt đầu chuyển trạng thái
    yield return null;
    
    // Lấy thông tin về trạng thái animation hiện tại
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    
    // Kiểm tra xem có đang trong animation "Die" không (giả sử tag hoặc name của state là "Die")
    while (!stateInfo.IsName("Die") || stateInfo.normalizedTime < 1.0f)
    {
        // Cập nhật thông tin trạng thái
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return null;
    }
    
    // Animation đã kết thúc, destroy game object
    Destroy(gameObject);
}

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}