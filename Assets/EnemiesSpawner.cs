using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject defaultEnemy;

    public EnemyScriptable[] enemiesKind;
    public int enemyPoolLength;
    private Stack<GameObject> enemyPool;

    #region GamePlayControlVariables
    float minX, maxX;
    #endregion

    private void Awake()
    {
        enemyPool = new Stack<GameObject>();
        FillStack();
    }
    void Start()
    {
        Debug.Log("Enemieskind length: " + enemiesKind.Length);
        SpawnEnemy(Random.Range(0, enemiesKind.Length));
    }

    void SetSpawnLimits()
    {
        minX = Camera.main.ScreenToWorldPoint(Camera.main.transform.position).x;
        maxX = minX;
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
        if (enemyPool.Count >= 0)
        {
            Vector3 pos = Vector3.zero;
            pos = GameManager.Instance_.player.transform.position;
            GameObject spawned = enemyPool.Pop();
            spawned.transform.parent = null;
            spawned.transform.position = pos;
            //Seteamos todo lo que debe tener el enemigo
            SetEnemyValues(enemyKind, ref spawned);
            spawned.SetActive(true);
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

        //Valores de componente SpriteRenderer
        enemyInstance.GetComponent<SpriteRenderer>().sprite = eScriptable.sprite;

        //Valores de componente animatorOverrideController
        enemyInstance.GetComponent<Animator>().runtimeAnimatorController = eScriptable.anim;

        //Ajustar collider a la imagen
        //enemyInstance.GetComponent<BoxCollider>().bounds.
    }

    private void DefineSpawnPos(ref GameObject enemyInstance)
    {

    }

    public void RecycleEnemy(GameObject go)
    {
        go.SetActive(false);
        go.transform.parent = this.transform.parent;
        enemyPool.Push(go);
    }

}
