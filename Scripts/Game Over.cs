using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public void Menu()
    {
        Debug.Log(" Menu ");
        SceneManager.LoadScene("Menu");
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
