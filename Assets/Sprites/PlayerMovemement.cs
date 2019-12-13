using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemement : MonoBehaviour
{
    public GameObject floor;
    [Range(10.0f, 100.0f)]
    public float moveSpeed = 65.0f;
    [Range(1.0f, 10.0f)]
    public float deltaPos = 1.0f;
    public bool moveWithPosition;
    [Header("Attack variables")]
    #region AttackVariables
    [Range(0.01f, 1.0f)]
    public float inputAttackTime;
    #endregion

    private float maxZ,minZ;
    Animator anim;
    SpriteRenderer sr;
    Rigidbody rb;
    GameManager myGameManager;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 size = floor.GetComponent<MeshCollider>().bounds.size;
        maxZ = floor.transform.position.z + size.z * 0.5f;
        minZ = maxZ - size.y;

        anim = this.transform.GetComponent<Animator>();
        sr = this.transform.GetComponent<SpriteRenderer>();
        rb = this.transform.GetComponent<Rigidbody>();
        myGameManager = GameManager.Instance_;

        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
 
    }
  
    protected bool Attack()
    {
        if (myGameManager.timeMouseUp < 0.1f)
        {
            //Atacar
            anim.Play("Attack");
            return true;
        }
        return false;
    }

    IEnumerator Move()
    {
        while(true)
        {
            Attack();
            float horiz = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            if (moveWithPosition)
            {
                //Movimiento horizontal
                Vector3 pos = this.transform.localPosition;
                pos.x += horiz * deltaPos * Time.deltaTime;
                //Movimiento vertical
                pos.z += vert * deltaPos * Time.deltaTime;
                //pos.z = Mathf.Clamp(pos.y, minZ, maxZ);
                this.transform.position = pos;

            }
            else
            {
                Vector3 vel = rb.velocity;
                vel.x = horiz * moveSpeed * Time.deltaTime;
                vel.z = vert * moveSpeed * Time.deltaTime;
                rb.velocity = vel;
            }
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
            yield return null;
        }
    }
}
