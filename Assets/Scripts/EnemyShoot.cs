using System;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("权重过渡速度")]
    public float fadeSpeed = 3f;
    
    private Animator animator;
    private Transform player;
    private PlayerHealth playerHealth;

    private float ikWeight = 0;

    private ShootingEffect shootingEffect;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag(GameConst.TAG_PLAYER).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        shootingEffect = GetComponentInChildren<ShootingEffect>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //是否处于射击状态
        bool isShooting = animator.GetCurrentAnimatorStateInfo(
            1).shortNameHash == GameConst.STATE_SHOOT;
        bool isRaising = animator.GetCurrentAnimatorStateInfo(
            1).shortNameHash == GameConst.STATE_RAISE;

        if (isShooting || isRaising)
        {
            ikWeight = Mathf.Lerp(ikWeight, 1, Time.deltaTime * fadeSpeed);
            
            //设置权重
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand,ikWeight);
            animator.SetLookAtWeight(ikWeight);
            
            //设置Ik位置
            animator.SetIKPosition(AvatarIKGoal.RightHand,player.position + Vector3.up * GameConst.PLAYER_BODY_HEIGHT);
            animator.SetLookAtPosition(player.position + Vector3.up * GameConst.PLAYER_BODY_HEIGHT);
        }
        else
        {
            ikWeight = Mathf.Lerp(ikWeight, 0, Time.deltaTime * fadeSpeed);
            //设置权重
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand,ikWeight);
            animator.SetLookAtWeight(ikWeight);
        }
    }

    /// <summary>
    /// 射击事件
    /// </summary>
    public void Shoot()
    {
        //启动播放特效协程
        StartCoroutine(shootingEffect.PlayEffect());
        //获取距离
        float distance = Vector3.Distance(player.position, transform.position);
        //造成伤害
        playerHealth.TakeDamage((10-distance) * 20 + 10);
    }
}
