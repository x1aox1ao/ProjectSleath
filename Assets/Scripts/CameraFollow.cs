using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("档位数量")]
    public int gear = 5;

    [Header("相机移动速度")]
    public float moveSpeed = 3f;
    [Header("相机转动速度")]
    public float turnSpeed = 5f;
    
    //跟随的目标
    private Transform followTarget;
    //方向向量
    private Vector3 dir;

    //所有可选的位置坐标
    private Vector3[] positions;

    //射线碰撞检测器
    private RaycastHit hit;

    private void Awake()
    {
        followTarget = GameObject.FindWithTag(GameConst.TAG_PLAYER).transform;

        if (gear < 2)
            gear = 2;
        
        positions = new Vector3[gear];
    }

    private void Start()
    {
        //求摄像机与跟随目标之间的方向向量
        dir = followTarget.position + Vector3.up * GameConst.PLAYER_BODY_HEIGHT - transform.position ;
    }

    private void Update()
    {
        Vector3 bestPos = followTarget.position + Vector3.up * GameConst.PLAYER_BODY_HEIGHT - dir;
        Vector3 badPos = followTarget.position + Vector3.up * GameConst.PLAYER_BODY_HEIGHT + Vector3.up * GameConst.OVERLOOKHEIGHT;

        positions[0] = bestPos;
        positions[positions.Length - 1] = badPos;

        for (int i = 1; i < positions.Length - 1; i++)
        {
            //通过插值求中间点的坐标
            positions[i] = Vector3.Lerp(bestPos,badPos,(float)i/(gear-1));
        }

        //观察点，初值就是最好的点
        Vector3 watchPoint = bestPos;
        
        //遍历可选坐标数组，找到能看到目标的最好的点
        for (int i = 0; i < positions.Length; i++)
        {
            if (CanSeeTarget(positions[i]))
            {
                //找到了合适的点
                watchPoint = positions[i];
                //跳出循环
                break;
            }
        }

        //平滑移动到最佳观察点
        transform.position = Vector3.Lerp(transform.position,
            watchPoint,Time.deltaTime * moveSpeed);

        //观察点指向目标的方向向量
        Vector3 lookDir = followTarget.position + Vector3.up * GameConst.PLAYER_BODY_HEIGHT - watchPoint;
        //将方向向量转换为四元数
        Quaternion targetQua = Quaternion.LookRotation(lookDir);
        //Lerp过去
        transform.rotation = Quaternion.Lerp(transform.rotation,
            targetQua,Time.deltaTime * turnSpeed);
        //欧拉角约束
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.y = 0;
        eulerAngles.z = 0;
        transform.eulerAngles = eulerAngles;
    }

    /// <summary>
    /// 是否可以看到目标
    /// </summary>
    /// <returns></returns>
    private bool CanSeeTarget(Vector3 watchPos)
    {
        //观察点指向跟随目标的方向向量
        Vector3 watchDir = followTarget.position + Vector3.up * GameConst.PLAYER_BODY_HEIGHT - watchPos;

        if (Physics.Raycast(watchPos, watchDir, out hit))
        {
            //如果射线检测到的目标是玩家
            if (hit.collider.CompareTag(GameConst.TAG_PLAYER))
            {
                return true;
            }
        }

        return false;
    }
}
