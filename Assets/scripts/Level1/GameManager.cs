using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public static GameManager Instance_
    {
        get
        {
            return instance_;
        }
        set
        {
            if(instance_ == null)
            {
                instance_ = value;
            }
        }
    }
    private static GameManager instance_;
    public GameObject player;
    [Range(0.01f, 0.5f)]
    public float zOffset = 0.01f;
    [Range(0.01f, 1.0f)]
    public float zColliderResizing = 1.0f;
    [Header("Objetos para crear despues de rotar")]
    public GameObject[] ObjectList;

    //Inputs
    [HideInInspector]
    public bool mouseInputDown;
    [HideInInspector]
    public float timeMouseDown;
    [HideInInspector]
    public float latestTimeMouseDown;
    [HideInInspector]
    public float timeMouseUp;

    //Score
    [SerializeField] private Text text_points;
    private int points;

    Vector3 playerDirection = Vector3.zero;
    public void OnReleaseMoveCallback(int move)
    {
        switch (move)
        {
            case 0: // arriba->W
                playerDirection.z = 0.0f;
                break;
            case 1: // izq->A
                playerDirection.x = 0.0f;
                break;
            case 2: // abajo->S
                playerDirection.z = 0.0f;
                break;
            case 3: // derecha->D
                playerDirection.x = 0.0f;
                break;
        }

        //Asignamos al movimiento de player
        player.GetComponent<PlayerMovement>().moveDirection = playerDirection;
    }

    public void OnMovesCallback(int move)
    {
        float magnitude = 1.0f;
        switch(move)
        {
            case 0: // arriba->W
                playerDirection.z = magnitude;
                break;
            case 1: // izq->A
                playerDirection.x = -magnitude;
                break;
            case 2: // abajo->S
                playerDirection.z = -magnitude;
                break;
            case 3: // derecha->D
                playerDirection.x = magnitude;
                break;
        }

        //Asignamos al movimiento de player
        player.GetComponent<PlayerMovement>().moveDirection = playerDirection;
    }

    public void AttackCallback()
    {
        player.GetComponent<PlayerMovement>().Attack();
    }
    private void Awake()
    {
        instance_ = this;
        mouseInputDown = false;
        timeMouseDown = 0.0f;
        latestTimeMouseDown = 0.0f;
        timeMouseUp = 0.0f;
        FullScreenButton();

        //--------------Score al inicio------------------
        points = 0;
        text_points.text = points.ToString();
    }
    private void FullScreenButton()
    {
        Canvas myCanvas = GetComponent<Canvas>();
        myCanvas.planeDistance = Camera.main.nearClipPlane + 0.01f;
        RectTransform r = transform.GetChild(0).GetComponent<RectTransform>();
        r.sizeDelta *= myCanvas.pixelRect.size * 1.0f;
        //r.size = myCanvas.pixelRect.size;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        timeMouseUp = 0.0f;
        Debug.Log("timeMouseUp");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //latestTimeMouseDown = timeMouseDown;
        timeMouseDown = 0.0f;
        Debug.Log("timeMouseDown");
    }
    IEnumerator InputManagement()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                timeMouseDown += Time.deltaTime;
            }else
            {
                timeMouseUp += Time.deltaTime;
            }
            yield return null;
        }
    }

    void Start()
    {
        StartCoroutine(InputManagement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNonRotatedColliders()
    {
        for(int i = 0; i< ObjectList.Length; i++)
        {

            BoxCollider bcChild = new GameObject().AddComponent<BoxCollider>();
            bcChild.name = ObjectList[i].name + "Collider";
            bcChild.transform.parent = ObjectList[i].transform;
            bcChild.transform.position = ObjectList[i].transform.position;

            Bounds parentBounds = ObjectList[i].transform.GetComponent<SpriteRenderer>().bounds;
            bcChild.size = parentBounds.size;
            float nearestZToCamera = bcChild.transform.position.z + bcChild.size.z;
            float playerRot = player.transform.rotation.eulerAngles.x;
            bcChild.size = new Vector3(bcChild.size.x * 0.7f, bcChild.size.y, Mathf.Cos( playerRot* Mathf.PI / 180.0f) * bcChild.size.y * 0.5f * zColliderResizing);
            bcChild.center -= new Vector3(0,0, bcChild.size.z * zOffset);
            //Z position is: 45 degrees cos de la mitad de su altura?
            //bcChild.center -= new Vector3(0,0,1) * ;

        }
        ResizePlayer();
    }

    public void ResizePlayer()
    {
        int childIndex = player.transform.childCount - 1;
        Debug.Log("childIndex: " + childIndex);
        BoxCollider bc = player.transform.GetChild(childIndex).GetComponent<BoxCollider>();
        bc.size -= new Vector3(0.2f * bc.size.x,0,0);
        bc.center += new Vector3(0,0, bc.size.z * 0.5f);
    }


}
