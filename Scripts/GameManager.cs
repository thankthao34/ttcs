using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager GM;

    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode attack { get; set; }

    public int enemiesKilled = 0; 

    void Awake()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }

        left = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        jump = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
        attack = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attackKey", "Q"));
    }

    void Start () {

    }

    void Update () {

    }

    public void AddEnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("Enemies killed: " + enemiesKilled); 
    }
}