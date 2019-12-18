using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCollider : MonoBehaviour
{
    PlayerMovement playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameManager.Instance_.player.GetComponent<PlayerMovement>();
        Physics.IgnoreCollision(playerScript.playerCollider, this.GetComponent<Collider>());
    }
    private void OnEnable()
    {  

    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider c = collision.collider;
        if (c.tag == "enemy")
        {
            c.GetComponent<Enemy>().DealDamage(playerScript.attack_dmg);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
