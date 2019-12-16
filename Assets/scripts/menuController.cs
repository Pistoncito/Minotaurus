using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{

    [SerializeField] private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio.Play();
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
