using System;
using System.Collections;
using UnityEngine;

public class LiftRaise : MonoBehaviour
{
    [Header("移动速度")]
    public float moveSpeed = 3f;
    [Header("过渡速度")]
    public float fadeSpeed = 3f;
    
    //电梯是否已经上升
    private bool liftHasRaise = false;

    private AudioSource audioSource;

    private float timer = 0;

    private Transform lift;
    private Transform player;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lift = transform.root;
        player = GameObject.FindWithTag(GameConst.TAG_PLAYER).transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag(GameConst.TAG_PLAYER))
            return;
        
        //计时器计时
        timer += Time.deltaTime;
        if(timer < 1)
            return;

        //如果电梯还没有上升
        if (!liftHasRaise)
        {
            //计时器归零
            timer = 0;
            //启动协程
            StartCoroutine(PlayEndGameAudio());
            liftHasRaise = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //如果电梯还没有上升
        if (!liftHasRaise)
        {
            timer = 0;
        }
    }

    /// <summary>
    /// 播放游戏结束音效
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayEndGameAudio()
    {
        //剥夺玩家的钥匙
        player.GetComponent<PlayerPacksack>().hasKey = false;
        
        //播放电梯上升音效
        audioSource.Play();

        while (timer < 8)
        {
            //计时器计时
            timer += Time.deltaTime;
            //电梯和玩家同时上升
            lift.position += Vector3.up * Time.deltaTime * moveSpeed;
            player.position += Vector3.up * Time.deltaTime * moveSpeed;
            if (timer > 6)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * fadeSpeed);
            }

            yield return new WaitForEndOfFrame();
        }
        
        //重新加载游戏
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
