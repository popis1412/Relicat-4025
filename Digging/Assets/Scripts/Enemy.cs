using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigidbody;

    int targetDirection = 1;
    float timer = 1f;
    float speed = 1f;

    int damage = 1;

    float attackCooldown = 1f;

    void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(LevelManager.instance.isRunning == true)
        {
            if (rigidbody != null)
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
        
    }

    public void EnemyDie() //�÷��̾�or��ź�� ȣ���ؼ� ���� ���̴� �Լ�
    {
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), true);

            PlayerController playerScript = collision.GetComponent<PlayerController>();
            if(playerScript != null && attackCooldown <= 0)
            {
                attackCooldown = 1f;

                //�÷��̾ ����� �Դ� �Լ� ȣ�� �ʿ�
                playerScript.TakeDamage(damage, transform);
            }
        }
        else if(collision.tag == "Enemy")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CircleCollider2D>(), true);

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), true);

            PlayerController playerScript = collision.GetComponent<PlayerController>();
            if(playerScript != null && attackCooldown <= 0)
            {
                attackCooldown = 1f;

                //�÷��̾ ����� �Դ� �Լ� ȣ�� �ʿ�
                playerScript.TakeDamage(damage, transform);
            }
        }
        else if(collision.tag == "Enemy")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), collision.GetComponent<CircleCollider2D>(), true);
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
