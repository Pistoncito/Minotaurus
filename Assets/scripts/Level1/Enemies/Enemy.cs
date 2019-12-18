using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public float dmg_dealt;
    [HideInInspector]
    public int score_given;
    [HideInInspector]
    public float hp;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float attack_range;

    [HideInInspector]
    public EnemiesSpawner spawnerReference;

    private Animator anim;
    private Rigidbody rb;
    private SpriteRenderer sr;
    private bool alive;
    // Start is called before the first frame update
    void Start()
    {
  
      
    }
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        alive = true;
        StartCoroutine(Behaviour());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SuicideAfter(float secs)
    {
        yield return new WaitForSeconds(secs);
        alive = false;
    }

    IEnumerator Behaviour()
    {
       //StartCoroutine(SuicideAfter(3.0f));
       GameObject player = GameManager.Instance_.player;
       while(alive)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) <= attack_range)
            {
                Attack();
            }
            else
            {
                MoveTowards(player.transform.position);
                CheckAnimation();
            }
       
            yield return null;
        }
        StartCoroutine(Die());
    }

    void MoveTowards(Vector3 to)
    {
        Vector3 dir = (to - this.transform.position).normalized;
        rb.velocity = dir * speed* Time.deltaTime;
    }

    IEnumerator Die()
    {
        rb.velocity = Vector3.zero;
        anim.Play("Die");
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            yield return null;
        }

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        //Debug.Log("MUERE " + this.name);
        GameManager.Instance_.AddScore(score_given);
        //yield return (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        spawnerReference.RecycleEnemy(this.gameObject);
    }

    void Attack()
    {
        rb.velocity = Vector3.zero;
        anim.Play("Attack");
    }
    void CheckAnimation()
    {
        float horiz, vert;
        horiz = rb.velocity.x;
        vert = rb.velocity.z;

        if (horiz != 0.0f || vert != 0.0f)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        if ((horiz < 0.0f && !sr.flipX) || (horiz > 0.0f && sr.flipX))
        {
            sr.flipX = !sr.flipX;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == GameManager.Instance_.player)
        {
            //daña al jugador
            GameManager.Instance_.player.GetComponent<PlayerMovement>().DealDamage(dmg_dealt);
        }
    }

    public void DealDamage(float dmg)
    {
        hp -= dmg;
        if(hp <= 0.0f)
        {
            alive = false;
        }
    }
}
