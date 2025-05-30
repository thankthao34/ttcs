using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    Transform menuPanel;  
    Event keyEvent;             
    Text buttonText;     
    KeyCode newKey;         
    bool waitingForKey;         

    void Start()
    {
        menuPanel = transform.Find("Menu");
        waitingForKey = false;

        // Cập nhật hiển thị phím ban đầu
        for (int i = 0; i < menuPanel.childCount; i++)
        {
            Text childText = menuPanel.GetChild(i).GetComponentInChildren<Text>();
            if (childText == null) continue;

            if (menuPanel.GetChild(i).name == "leftKey")
            {
                childText.text = GameManager.GM.left.ToString();
            }
            else if (menuPanel.GetChild(i).name == "rightKey")
            {
                childText.text = GameManager.GM.right.ToString();
            }
            else if (menuPanel.GetChild(i).name == "jumpKey")
            {
                childText.text = GameManager.GM.jump.ToString();
            }
            else if (menuPanel.GetChild(i).name == "attackKey")
            {
                childText.text = GameManager.GM.attack.ToString();
            }
        }
    }

    void Update()
    {
    }

    void OnGUI()
    {
        keyEvent = Event.current;  

        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)  
    {
        if (!waitingForKey)
        {
            StartCoroutine(AssignKey(keyName));
        }
    }

    public void SendText(Text text)  
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()  
    {
        while (!keyEvent.isKey)
        {
            yield return null;
        }
    }

    public IEnumerator AssignKey(string keyName)  
    {
        waitingForKey = true;
        yield return WaitForKey();
        switch (keyName)
        {
            case "jump":
                GameManager.GM.jump = newKey;
                buttonText.text = GameManager.GM.jump.ToString();
                PlayerPrefs.SetString("jumpKey", GameManager.GM.jump.ToString());
                break;
            case "right":
                GameManager.GM.right = newKey;
                buttonText.text = GameManager.GM.right.ToString();
                PlayerPrefs.SetString("rightKey", GameManager.GM.right.ToString());
                break;
            case "left":
                GameManager.GM.left = newKey;
                buttonText.text = GameManager.GM.left.ToString();
                PlayerPrefs.SetString("leftKey", GameManager.GM.left.ToString());
                break;
            case "attack":
                GameManager.GM.attack = newKey;
                buttonText.text = GameManager.GM.attack.ToString();
                PlayerPrefs.SetString("attackKey", GameManager.GM.attack.ToString());
                break;
        }
        yield return null;
    }
}