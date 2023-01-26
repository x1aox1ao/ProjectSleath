using System;
using UnityEngine;

public class BigDoorController : MonoBehaviour
{
    public AudioClip doorOpenClip;
    public AudioClip doorAccessDeniedClip;
    
    private Animator animator;
    private Animator innerDoorAnimator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        innerDoorAnimator = GameObject.FindWithTag(
            GameConst.TAG_INNERDOOR).GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(GameConst.TAG_PLAYER))
            return;
        if (other.GetComponent<PlayerPacksack>().hasKey)
        {
            //开门动画
            animator.SetBool(GameConst.PARAMETER_DOOROPEN,true);
            innerDoorAnimator.SetBool(GameConst.PARAMETER_DOOROPEN,true);
            //播放开门声音
            AudioSource.PlayClipAtPoint(doorOpenClip,transform.position);
        }
        else
        {
            //播放拒绝开门声音
            AudioSource.PlayClipAtPoint(doorAccessDeniedClip,transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(GameConst.TAG_PLAYER))
            return;
        //如果门开着
        if (animator.GetBool(GameConst.PARAMETER_DOOROPEN))
        {
            //关门动画
            animator.SetBool(GameConst.PARAMETER_DOOROPEN,false);
            innerDoorAnimator.SetBool(GameConst.PARAMETER_DOOROPEN,false);
        }
    }
}
