using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
     [SerializeField] protected Slider slider; // Thanh máu UI
    [SerializeField] private Enemy1 Enemy1; // Script của boss để lấy máu

    void Start()
    {
        if (slider == null || Enemy1 == null)
        {
            Debug.LogError("Health Slider hoặc Boss Script chưa được gán!");
            return;
        }
        slider.maxValue = 3;
        slider.value = Enemy1.maxHealth;

        
    }

    void Update()
    {
        
        // Cập nhật giá trị máu
        slider.value = Enemy1.maxHealth;

    }
}
