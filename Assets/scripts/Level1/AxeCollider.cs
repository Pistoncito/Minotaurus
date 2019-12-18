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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            Debug.Log("Collide with enemy");
            other.GetComponent<Enemy>().DealDamage(playerScript.attack_dmg);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
