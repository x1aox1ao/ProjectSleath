using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("追捕速度")]
    public float chasingSpeed = 6f;
    [Header("巡逻速度")]
    public float patrollingSpeed = 2f;
    [Header("等待时间")]
    public float waitingInterval = 3f;
    [Header("巡逻路点")]
    public Transform[] wayPoints;

    private PlayerHealth playerHealth;
    private EnemySighting enemySighting;
    private AlarmSystem alarmSystem;
    private NavMeshAgent navMeshAgent;

    private float timer;
    //路点索引号
    private int wayPointIndex;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemySighting = GetComponent<EnemySighting>();
        playerHealth = GameObject.FindWithTag(
            GameConst.TAG_PLAYER).GetComponent<
            PlayerHealth>();
        alarmSystem = GameObject.FindWithTag(
            GameConst.TAG_GAMECONTROLLER).GetComponent<
            AlarmSystem>();
    }

    private void Update()
    {
        if (playerHealth.hp <= 0)
        {
            Patrolling();
            return;
        }
                
        //能够看到玩家，且玩家活着
        if (enemySighting.playerInSight)
        {
            //射击
            Shooting();
        }
        //拥有一个玩家可能出现的坐标位置
        else if(enemySighting.personalAlarmPosition
            != alarmSystem.safePosition)
        {
            //追捕
            Chasing();
        }
        else
        {
            //巡逻
            Patrolling();
        }
    }

    /// <summary>
    /// 射击
    /// </summary>
    private void Shooting()
    {
        //停止导航
        navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// 追捕
    /// </summary>
    private void Chasing()
    {
        //恢复导航
        navMeshAgent.isStopped = false;
        //设置导航速度
        navMeshAgent.speed = chasingSpeed;
        //导航到警报位置
        navMeshAgent.SetDestination(
            enemySighting.personalAlarmPosition);

        //突发事件：出现路障无法到达指定位置
        if(navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            //取消全局警报
            alarmSystem.alarmPosition = alarmSystem.safePosition;
            //取消私人警报
            enemySighting.personalAlarmPosition = alarmSystem.safePosition;
        }

        //判断如果到达了导航目标
        if (navMeshAgent.remainingDistance -
            navMeshAgent.stoppingDistance < 0.05f)
        {
            //计时器计时
            timer += Time.deltaTime;
            if (timer > waitingInterval)
            {
                //取消全局警报
                alarmSystem.alarmPosition = alarmSystem.safePosition;
                //取消私人警报
                enemySighting.personalAlarmPosition = alarmSystem.safePosition;
                //计时器归零
                timer = 0;
            }
        }
        else
        {
            //计时器归零
            timer = 0;
        }
    }

    /// <summary>
    /// 巡逻
    /// </summary>
    private void Patrolling()
    {
        //恢复导航
        navMeshAgent.isStopped = false;
        //设置导航速度
        navMeshAgent.speed = patrollingSpeed;
        //设置导航目标
        navMeshAgent.SetDestination(wayPoints[wayPointIndex].position);
        //判断如果到达了导航目标
        if (navMeshAgent.remainingDistance -
            navMeshAgent.stoppingDistance < 0.05f)
        {
            //计时器计时
            timer += Time.deltaTime;
            if (timer > waitingInterval)
            {
                //切换路点编号（防止越界）
                wayPointIndex = ++wayPointIndex % wayPoints.Length;
                //计时器归零
                timer = 0;
            }
        }
        else
        {
            //计时器归零
            timer = 0;
        }
        
    }
}