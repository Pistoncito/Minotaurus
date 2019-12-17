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
    EnemiesSpawner spawnerReference;

    private Animator anim;
    private Rigidbody rb;
    private bool alive;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        alive = true;
        StartCoroutine(Behaviour());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Behaviour()
    {
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
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
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

    }
}
