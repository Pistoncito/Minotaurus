using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject defaultEnemy;
    public GameObject spawnSurface;
    public EnemyScriptable[] enemiesKind;
    public int enemyPoolLength;
    private Stack<GameObject> enemyPool;
    private LinkedList<GameObject> enemiesSpawned;
    #region GamePlayControlVariables
    public float minX, maxX;
    public float minZ, maxZ;
    #endregion

    private void Awake()
    {
        enemyPool = new Stack<GameObject>();
        enemiesSpawned = new LinkedList<GameObject>();
        FillStack();
    }

    void Start()
    {
        //SpawnEnemy(Random.Range(0, enemiesKind.Length));
        StartCoroutine(SpawnRandom());
    }

    IEnumerator SpawnRandom()
    {
        yield return new WaitForSeconds(3.0f);
        while (true)
        {
            AddToEnemiesSpawned(SpawnEnemy(Random.Range(0, enemiesKind.Length)));
            yield return new WaitForSeconds(5.0f);
        }
    }

    void AddToEnemiesSpawned(GameObject spawned)
    {
        if(spawned != null)
        {
            //Ignoro colisiones con otros enemigos
            BoxCollider bc = spawned.GetComponent<BoxCollider>();
            foreach (GameObject e in enemiesSpawned)
            {
                BoxCollider bce = e.GetComponent<BoxCollider>();
                Physics.IgnoreCollision(bce, bc);
            }
            //Lo meto en la lista de spawneds
            enemiesSpawned.AddLast(spawned);
        }
   
    }
    void FillStack()
    {
        //Crear el objeto y disable
        for (int i = 0; i < enemyPoolLength; i++)
        {
            GameObject go = Instantiate(defaultEnemy);
            go.transform.parent = this.transform;
            go.SetActive(false);
            enemyPool.Push(go);
        }
    }

    GameObject SpawnEnemy(int enemyKind)
    {
        if (enemyPool.Count > 0)
        {
            GameObject spawned = enemyPool.Pop();
            spawned.transform.parent = null;
            //Seteamos todo lo que debe tener el enemigo
            SetEnemyValues(enemyKind, ref spawned);
            spawned.SetActive(true);
            Debug.Log("SPAWN " + spawned.name);
            return spawned;
        }

        return null;
    }

    private void SetEnemyValues(int enemy, ref GameObject enemyInstance)
    {
        DefineSpawnPos(ref enemyInstance);
        //Valores de script
        Enemy eScript = enemyInstance.GetComponent<Enemy>();
        EnemyScriptable eScriptable = enemiesKind[enemy];
        eScript.dmg_dealt = eScriptable.dmg_dealt;
        eScript.score_given = eScriptable.score_given;
        eScript.hp = eScriptable.hp;
        eScript.speed = eScriptable.speed;
        eScript.attack_range = eScriptable.attack_range;
        enemyInstance.name = eScriptable.name;
        eScript.spawnerReference = this;

        //Valor del tag para las colisiones mas sencillas con el hacha
        enemyInstance.tag = "enemy";
        //Valores de componente SpriteRenderer
        enemyInstance.GetComponent<SpriteRenderer>().sprite = eScriptable.sprite;

        //Valores de componente animatorOverrideController
        enemyInstance.GetComponent<Animator>().runtimeAnimatorController = eScriptable.anim;

        //Ajustar collider a la imagen
        //enemyInstance.GetComponent<BoxCollider>().bounds.
    }

    private void DefineSpawnPos(ref GameObject enemyInstance)
    {
        Vector3 playerPos = GameManager.Instance_.player.transform.position;
        enemyInstance.transform.position = playerPos;
        Vector3 pos = enemyInstance.transform.localPosition;

        float xOffset = Random.Range(minX, maxX);
        pos.x += xOffset;
        float zOffset = Random.Range(minZ, maxZ);
        pos.z += zOffset;
        enemyInstance.transform.localPosition = new Vector3(pos.x, playerPos.y, pos.z);
    }

    public void RecycleEnemy(GameObject go)
    {
        go.transform.position = this.transform.position;
        go.transform.localPosition = Vector3.zero;

        Debug.Log("Removed " + go.name + "?: "+  enemiesSpawned.Remove(go));
        go.SetActive(false);
        go.transform.parent = this.transform.parent;
        enemyPool.Push(go);
    }

}
