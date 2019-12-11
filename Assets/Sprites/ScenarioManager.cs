using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        RotateAllLikePlayer(this.transform);
        GameManager.Instance_.CreateNonRotatedColliders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RotateAllLikePlayer(Transform t)
    {
        int childs = t.childCount;
        if (t.childCount == 0)
        {
            t.localRotation = player.rotation;
            //Debug.Log("Rotated: " + t.name);
        }
        else
        {  
            for (int i = 0; i < t.childCount; i++)
            {
                RotateAllLikePlayer(t.GetChild(i));
            }
        }
       
    }
}
