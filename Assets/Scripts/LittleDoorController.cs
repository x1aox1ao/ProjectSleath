using System;
using UnityEngine;

public class LittleDoorController : MonoBehaviour
{
    //计数器
    private int counter = 0;

    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
            return;
        
        if (other.CompareTag(GameConst.TAG_PLAYER)
            || other.CompareTag(GameConst.TAG_ENEMY))
        {
            
            //第一个进入触发器的人，开门
            if (++counter == 1)
            {
                animator.SetBool(GameConst.PARAMETER_DOOROPEN,true);
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.isTrigger)
            return;
        
        if (other.CompareTag(GameConst.TAG_PLAYER)
            || other.CompareTag(GameConst.TAG_ENEMY))
        {
            //最后一个离开触发器的人，关门
            if (--counter == 0)
            {
                animator.SetBool(GameConst.PARAMETER_DOOROPEN,false);
                audioSource.Play();
            }
        }
    }
}
