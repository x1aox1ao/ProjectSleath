using System;
using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [Header("拾取钥匙音效")]
    public AudioClip pickupClip;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConst.TAG_PLAYER))
        {
            //玩家拾取钥匙
            other.GetComponent<PlayerPacksack>().hasKey = true;
            //播放音效
            AudioSource.PlayClipAtPoint(pickupClip,transform.position);
            //销毁自己
            Destroy(gameObject);
        }
    }
}
