using System;
using UnityEngine;

public class TriggerAlarm : MonoBehaviour
{
    private AlarmSystem alarmSystem;

    private void Awake()
    {
        alarmSystem = GameObject.FindWithTag(
            GameConst.TAG_GAMECONTROLLER).GetComponent<AlarmSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConst.TAG_PLAYER))
        {
            //赋值警报位置，触发全局警报
            alarmSystem.alarmPosition = other.transform.position;
        }
    }
}