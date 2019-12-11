using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject target;
    [Range(1.0f, 10.0f)]
    public float distance;
    public Vector3 targetOffset;
    private float zTargetInit;

    [Range(0.0f, 1.0f)]
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        zTargetInit = target.transform.position.z;
        PositionCamera();
        StartCoroutine(FollowTarget());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FollowTarget()
    {
        while(true)
        {
            Vector3 pos = this.transform.position;
            pos.x = Mathf.Lerp(pos.x, target.transform.position.x, delay);

            this.transform.position = pos;
            yield return new WaitForSeconds(0.3f);
        }
    }
    void PositionCamera()
    {
        //Rotation
        //this.transform.rotation = target.transform.rotation;

        //position
        Vector3 cameraPos = this.transform.position;
        Vector3 targetPos = target.transform.position;
        cameraPos.z = zTargetInit - distance;
        cameraPos.x = targetPos.x + targetOffset.x;
        cameraPos.y = targetPos.y + targetOffset.y;
        //Movimiento en y para subsanar la rotacion
        float eulerRot = this.transform.rotation.eulerAngles.x;
        cameraPos.y += Mathf.Sin(Mathf.PI * eulerRot/ 180.0f)*2.0f;
      
        this.transform.position = cameraPos;

    
    }
}
