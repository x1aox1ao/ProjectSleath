using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("动画参数渐变时间")]
    public float dampTime = 1f;
    [Header("转身速度")]
    public float turnSpeed = 10;
    [Header("喊叫声音片段")]
    public AudioClip shoutClip;
    
    //虚拟轴
    private float hor, ver;
    //虚拟按键
    private bool sneak, attract;
    //动画组件
    private Animator _animator;
    //声音组件
    private AudioSource _audioSource;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        /*if (playerHealth.hp <= 0)
            return;*/

        hor = Input.GetAxis(GameConst.AXIS_HORIZONTAL);
        ver = Input.GetAxis(GameConst.AXIS_VERTICAL);
        sneak = Input.GetButton(GameConst.BUTTON_SNEAK);

        //shout
        attract = Input.GetButtonDown(GameConst.BUTTON_ATTRACT);
        
        //如果按下了任意一个方向键
        if (hor != 0 || ver != 0)
        {
            //设置动画参数，切换到Locomotion
            _animator.SetFloat(GameConst.PARAMETER_SPEED,
                5.66f,dampTime,Time.deltaTime);
            //玩家转身
            PlayerRotate();
        }
        else
        {
            //直接切换到Idle
            _animator.SetFloat(GameConst.PARAMETER_SPEED,1.4f);
        }
        
        //设置虚拟按键，是否潜行
        _animator.SetBool(GameConst.PARAMETER_SNEAK,sneak);

        if (attract)
        {
            //触发喊叫
            _animator.SetTrigger(GameConst.PARAMETER_SHOUT);
        }

        AudioControl();
    }

    private void PlayerRotate()
    {
        //方向向量
        Vector3 dir = new Vector3(hor,0,ver);
        //将方向向量转换为四元数
        Quaternion targetQua = Quaternion.LookRotation(dir);
        //Lerp过去
        transform.rotation = Quaternion.Lerp(transform.rotation,
            targetQua, Time.deltaTime * turnSpeed);
    }

    /// <summary>
    /// 播放喊叫音效
    /// </summary>
    public void PlayShoutAudio()
    {
        AudioSource.PlayClipAtPoint(shoutClip,transform.position);
    }
    
    private void AudioControl()
    {
        //判断玩家当前是否处于Locomotion状态
        if (_animator.GetCurrentAnimatorStateInfo(
            0).shortNameHash == GameConst.STATE_LOCOMOTION)
        {
            //如果声音没有播放
            if (!_audioSource.isPlaying)
            {
                //播放
                _audioSource.Play();
            }
        }
        else
        {
            //停止播放
            _audioSource.Stop();
        }
    }
}
