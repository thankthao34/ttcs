
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float duration;

    public void ShakeCamera(){
        CameraShake.instance.Shake(intensity, duration);
    }
}
