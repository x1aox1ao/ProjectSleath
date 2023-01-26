using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("初始血量")]
    public float hp = 100;
    [Header("游戏结束音效")]
    public AudioClip endGameClip;

    private Animator animator;

    //玩家是否还活着
    private bool playerAlive = true;

    private Animator endGameScreenEffect;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 计算伤害
    /// </summary>
    public void TakeDamage(float damage)
    {
        //扣血
        hp -= damage;
        
        //判断是否死亡
        if (hp <= 0 && playerAlive)
        {
            playerAlive = false;
            //触发死亡动画
            animator.SetTrigger(GameConst.PARAMETER_DEAD);
            //启动游戏结束协程
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        //播放游戏结束音效
        AudioSource.PlayClipAtPoint(endGameClip,transform.position);

/*        //找到结束游戏屏幕特效
        endGameScreenEffect = GameObject.FindWithTag(
            GameConst.TAG_FINISH).GetComponent<Animator>();

        //触发播放
        endGameScreenEffect.SetTrigger(GameConst.PARAMETER_ENDGAME);*/
        yield return new WaitForSeconds(endGameClip.length);
        //重新加载
        SceneManager.LoadScene(0);
    }
}
