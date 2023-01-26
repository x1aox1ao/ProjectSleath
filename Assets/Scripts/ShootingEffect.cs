using System.Collections;
using UnityEngine;

public class ShootingEffect : MonoBehaviour
{
    private Transform player;
    
    private LineRenderer lineRenderer;
    private Light light;
    private AudioSource audioSource;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        light = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindWithTag(GameConst.TAG_PLAYER).transform;
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    public IEnumerator PlayEffect()
    {
        //设置线性渲染器的顶点个数
        lineRenderer.positionCount = 2;
        //设置顶点坐标
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,player.position
            + Vector3.up * GameConst.PLAYER_BODY_HEIGHT);
        //打开灯光
        light.enabled = true;
        //播放音效
        audioSource.Play();
        
        yield return new WaitForSeconds(0.1f);
        
        //关闭激光
        lineRenderer.positionCount = 0;
        //关闭灯光
        light.enabled = false;
    }
}
