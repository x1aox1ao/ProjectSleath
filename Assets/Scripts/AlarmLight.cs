using System;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    [Header("警报开关")]
    public bool alarmOn;
    [Header("光强渐变速度")]
    public float fadeSpeed = 3f;

    //灯光组件
    private Light alarmLight;
    //低光强
    private float lowIntensity = 0;
    //高光强
    private float highIntensity = 3f;
    //目标光强
    private float targetIntensity;

    private void Awake()
    {
        alarmLight = GetComponent<Light>();
        targetIntensity = highIntensity;
    }

    private void Update()
    {
        if (alarmOn)
        {
            //渐变到目标光强，Lerp类似三角函数cos
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 
                targetIntensity, Time.deltaTime * fadeSpeed);
            //判断是否已经到达目标光强
            if (Mathf.Abs(alarmLight.intensity - targetIntensity) < 0.1f)
            {
                //切换目标光强
                targetIntensity = targetIntensity == highIntensity ?
                    lowIntensity : highIntensity;
            }
        }
        else
        {
            //渐变到低光强
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 
                lowIntensity, Time.deltaTime * fadeSpeed);
            if (alarmLight.intensity < 0.1f)
            {
                alarmLight.intensity = 0;
            }
        }
    }
}
