using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneMangeMent : MonoBehaviour
{
    public void Play(){
        SceneManager.LoadScene("Level1");
    }

    public void Exit(){
        Debug.Log("Exit");
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");

    }
    public void NextBttn()
    {
        Debug.Log("Load next level");
    }
    public void SetButton()
    {
        SceneManager.LoadScene("SetButton");
    }
    
}
