using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="New Enemy")]
public class EnemyScriptable : ScriptableObject
{
    public new string name;
    public float dmg_dealt;
    public int score_given;
    public float hp;
    public float speed;
    public float attack_range;
    public Sprite sprite;
    public AnimatorOverrideController anim;
    // Start is called before the first frame update
    void Start()
    {
    }

}
