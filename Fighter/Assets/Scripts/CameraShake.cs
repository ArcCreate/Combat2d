using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance {  get; private set; }
    private CinemachineVirtualCamera cm;
    private float timer;
    private void Awake()
    {
        instance = this;
        cm = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin noise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = intensity;
        timer = time;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin noise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                noise.m_AmplitudeGain = 0f;
            }
        }
    }

}