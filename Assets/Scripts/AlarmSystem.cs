using System;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    //警报坐标
    public Vector3 alarmPosition = new Vector3(100,100,100);
    //[HideInInspector]
    //安全坐标
    public Vector3 safePosition = new Vector3(100,100,100);
    //[HideInInspector]
    //警报灯管理
    private AlarmLight alarmLight;
    //主灯光
    private Light mainLight;
    //普通背景音乐
    private AudioSource normalAudio;
    //紧张气氛的背景音乐
    private AudioSource panicAudio;
    //喇叭音效
    private AudioSource[] sirens;

    //设置的目标值
    private byte targetValue;
    
    private void Awake()
    {
        alarmLight = GameObject.FindWithTag(
            "alarm_Light").GetComponent<AlarmLight>();
        mainLight = GameObject.FindWithTag(
            "main_Light").GetComponent<Light>();
        normalAudio = GetComponent<AudioSource>();
        panicAudio = transform.GetChild(0).GetComponent<AudioSource>();

        //先找到六个喇叭的游戏对象
        GameObject[] sirenObjs = GameObject.FindGameObjectsWithTag("Siren");
        sirens = new AudioSource[sirenObjs.Length];
        for (int i = 0; i < sirenObjs.Length; i++)
        {
            sirens[i] = sirenObjs[i].GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        //将警报坐标，转换成警报或非警报下的目标数值
        targetValue = Convert.ToByte(alarmPosition != safePosition);
        //设置警报灯
        alarmLight.alarmOn = alarmPosition != safePosition;
        //设置主灯光
        mainLight.intensity =
            Mathf.Lerp(mainLight.intensity, 1-targetValue,
                Time.deltaTime * alarmLight.fadeSpeed);
        //开启紧张气氛的背景音乐
        panicAudio.volume = 
            Mathf.Lerp(panicAudio.volume,targetValue,
                Time.deltaTime * alarmLight.fadeSpeed);
        //开启喇叭音效
        for (int i = 0; i < sirens.Length; i++)
        {
            sirens[i].volume = Mathf.Lerp(sirens[i].volume,
                targetValue,Time.deltaTime * alarmLight.fadeSpeed);
        }
        //关闭普通的背景音乐
        normalAudio.volume = Mathf.Lerp(normalAudio.volume,
            1-targetValue,Time.deltaTime * alarmLight.fadeSpeed);
    }
}
