using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public UnityEngine.UI.Image hpBar;
    public SphereCollider axeCollider;
    public GameObject floor;
    [Range(1.0f, 900.0f)]
    public float moveSpeed = 65.0f;
    [Range(1.0f, 10.0f)]
    public float deltaPos = 1.0f;
    public bool moveWithPosition;
    [Header("Attack variables")]
    #region AttackVariables
    [Range(0.01f, 1.0f)]
    public float inputAttackTime;
    #endregion

    #region HpVars
    public float maxHp;
    private float currentHp;
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

        //HP vars
        currentHp = maxHp;

        //StartCoroutine(Move());
        StartCoroutine(BehaviourOnMobile());
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    public bool atacking = false;
    public Vector3 moveDirection = Vector3.zero;

    private void CheckAnimationDisplay()
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
            //Girar el collider

            Vector3 aux = axeCollider.transform.localPosition;
            aux.x *= -1.0f;
            axeCollider.transform.localPosition = aux;
        }

    }
    public void Attack()
    {
        anim.Play("Attack");
    }
    IEnumerator BehaviourOnMobile()
    {
        while(!GameManager.Instance_.endOfGame)
        {
            
            //Priorizar atacar

            if(moveWithPosition)
            {
                Vector3 pos = this.transform.localPosition;
                pos += moveDirection * deltaPos * Time.deltaTime;
                this.transform.localPosition = pos;
            }else
            {
                rb.velocity = moveDirection * moveSpeed * Time.deltaTime;
            }
            CheckAnimationDisplay();
            yield return null;
        }
        rb.velocity = Vector3.zero;
    }
    /*
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
    */

    public void DealDamage(float dmg)
    {
        //StopCoroutine(UpdateHp(dmg));//La paro por si estaba ejecutandose ya una
        StartCoroutine(UpdateHp(dmg));
        CheckIfDie();
    }

    IEnumerator UpdateHp(float dmg)
    {
        float lastPercentage = currentHp/maxHp;
        currentHp -= dmg;
        float currentPercentage = currentHp / maxHp;
        float inTime = 1.0f;
        float timer = 0.0f;
        while(timer < inTime)
        {
            timer += Time.deltaTime;
            float percentageDisplay = Mathf.Lerp(lastPercentage, currentPercentage, timer);
            hpBar.fillAmount = percentageDisplay;
            yield return null;
        }
    }

    private void CheckIfDie()
    {
        if(currentHp <= 0.0f)
        {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
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
        GameManager.Instance_.endOfGame = true;
    }


}
