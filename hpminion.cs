using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpminion : MonoBehaviour
{
     [SerializeField] protected Slider slider; // Thanh máu UI
    [SerializeField] private minionPrefab minion; // Script của boss để lấy máu

    void Start()
    {
        if (slider == null || minion == null)
        {
            Debug.LogError("Health Slider hoặc Boss Script chưa được gán!");
            return;
        }
        slider.maxValue = 2;
        slider.value = minion.maxHealth;

        
    }

    void Update()
    {
        
        // Cập nhật giá trị máu
        slider.value = minion.maxHealth;

    }
}
