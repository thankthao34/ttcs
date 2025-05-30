using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpBoss : MonoBehaviour
{
     [SerializeField] protected Slider slider; // Thanh máu UI
    [SerializeField] private Undead_Boss boss; // Script của boss để lấy máu

    void Start()
    {
        if (slider == null || boss == null)
        {
            Debug.LogError("Health Slider hoặc Boss Script chưa được gán!");
            return;
        }
        slider.maxValue = 30;
        slider.value = boss.maxHealth;

        
    }

    void Update()
    {
        
        // Cập nhật giá trị máu
        slider.value = boss.maxHealth;

    }
}
