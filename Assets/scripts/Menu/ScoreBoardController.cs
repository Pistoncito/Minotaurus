using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoardController : MonoBehaviour
{
    public UnityEngine.UI.Text score;
    // Start is called before the first frame update
    void Start()
    {
        score.text = GameManager.Instance_.points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void change_scene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
