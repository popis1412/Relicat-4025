using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    Rigidbody2D rigidbody;

    int targetDirection = 1;
    float timer = 1f;
    float speed = 1f;

    int damage = 1;

    float attackCooldown = 1f;

    void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
        if(player != null)
        {
            if(timer - Time.deltaTime < 0)
            {
                timer = 1f;
                if (player.transform.position.x > this.gameObject.transform.position.x)
                    targetDirection = 1;
                else
                    targetDirection = -1;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }


        if(rigidbody != null)
        {
            rigidbody.velocity = new Vector2(speed * targetDirection, rigidbody.velocity.y);
        }

        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        if (targetDirection > 0)
        {
            this.gameObject.transform.localScale = new Vector3(-0.75f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(0.75f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
    }

    public void EnemyDie() //플레이어or폭탄이 호출해서 적을 죽이는 함수
    {
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerScript = collision.GetComponent<PlayerController>();

        if(collision.tag == "Player")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), true);

        if(playerScript != null && attackCooldown <= 0)
        {
            attackCooldown = 1f;

            //플레이어가 대미지 입는 함수 호출 필요
            playerScript.TakeDamage(damage, transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController playerScript = collision.GetComponent<PlayerController>();

        if (collision.tag == "Player")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), true);

        if (playerScript != null && attackCooldown <= 0)
        {
            attackCooldown = 1f;

            //플레이어가 대미지 입는 함수 호출 필요
            playerScript.TakeDamage(damage, transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            foreach(ContactPoint2D contact in collision.contacts)
            {
                if(Mathf.Abs(contact.normal.x) > 0.8f)
                {
                    timer = 1f;
                    if (contact.normal.x > 0)
                        targetDirection = 1;
                    else if (contact.normal.x < 0)
                        targetDirection = -1;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.8f)
                {
                    timer = 1f;
                    if (contact.normal.x > 0)
                        targetDirection = 1;
                    else if (contact.normal.x < 0)
                        targetDirection = -1;
                }
            }
        }
    }
}
