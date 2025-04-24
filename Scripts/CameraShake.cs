using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class CameraShake : MonoBehaviour
{
    #region Singleton
        
    public static CameraShake instance;
    [SerializeField] CinemachineVirtualCamera vCam;
    private float ShakeTime;

    private void Awake()
    {
        if(instance == null){
        instance = this;

        }
        else{
            Destroy(this);
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (ShakeTime >0f){
            ShakeTime -=Time.deltaTime;
        }

        if(ShakeTime <=0f){
            CinemachineBasicMultiChannelPerlin perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0f;
        }
    }

    public void Shake(float intensity, float duration){
        ShakeTime = duration;
        CinemachineBasicMultiChannelPerlin perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
    }
}
