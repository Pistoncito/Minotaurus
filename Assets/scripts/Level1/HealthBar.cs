using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private float hp;
    private const float HP_MAX = 100.0f;

    private void Awake()
    {
        healthBar = transform.Find("health").GetComponent<Image>();

        healthBar.fillAmount = 1.0f;
    }

    private void Update()
    {
        /*
        healthBar.fillAmount = hp;

        hp += 1.0f * Time.deltaTime;
        hp = Mathf.Clamp(hp, 0.0f, HP_MAX);
        */

    }
}
