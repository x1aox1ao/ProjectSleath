using System;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("被控制的激光")]
    public GameObject controledLaser;
    [Header("解锁的材质球")]
    public Material unlockMat;
    
    private MeshRenderer screenMeshRenderer;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        screenMeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    //触发器触发后
    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag(GameConst.TAG_PLAYER))
            return;
        if (Input.GetButtonDown(GameConst.BUTTON_SWITCH))
        {
            //如果控制的激光已经关掉了
            if(!controledLaser.activeSelf)
                return;
            //关闭控制的激光
            controledLaser.SetActive(false);
            //更换解锁材质
            screenMeshRenderer.material = unlockMat;
            //播放声音
            audioSource.Play();
        }
    }
}
