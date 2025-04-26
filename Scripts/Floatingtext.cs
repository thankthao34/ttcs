
using UnityEngine;

public class Floatingtext : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    // Start is called before the first frame update
    void Start()
    {
        int number = Random.Range(1, 3);
        textMesh.text = number.ToString();
        
    }
}
