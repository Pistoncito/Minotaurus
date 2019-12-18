using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ayuda");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collider playerCollider = GameManager.Instance_.player.GetComponent<PlayerMovement>().playerCollider;
        if (collision.collider == playerCollider)
        {
            GameManager.Instance_.change_scene();
        }
    }
}
