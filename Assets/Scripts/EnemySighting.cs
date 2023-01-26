using UnityEngine;
using UnityEngine.AI;

public class EnemySighting : MonoBehaviour
{
    [Header("视角范围")]
    [Range(90,180)]
    public float viewOfField = 130;
    //每个机器人所掌握的警报坐标
    public Vector3 personalAlarmPosition;
    [Header("能否看到玩家")]
    public bool playerInSight;
    //上一帧的警报坐标
    private Vector3 previousAlarmPosition;

    private AlarmSystem alarmSystem;

    private Transform player;

    private Animator playerAnimator;

    //当前机器人指向玩家的方向向量
    private Vector3 dir;

    private RaycastHit hit;


    private NavMeshAgent navMeshAgent;

    private NavMeshPath path;
    //触发器
    private SphereCollider trigger;
    
    private void Awake()
    {
        trigger = GetComponent<SphereCollider>();
        path = new NavMeshPath();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag(
            GameConst.TAG_PLAYER).transform;
        playerAnimator = player.GetComponent<Animator>();
        alarmSystem = GameObject.FindWithTag(
            GameConst.TAG_GAMECONTROLLER).GetComponent<AlarmSystem>();
        //初始警报位置为安全坐标
        personalAlarmPosition = alarmSystem.safePosition;
        previousAlarmPosition = alarmSystem.safePosition;
    }

    private void Update()
    {
        //上一帧的全局警报与当前帧的全局警报进行比较
        if (previousAlarmPosition != alarmSystem.alarmPosition)
        {
            //如果发生了变化，更新全局警报到私人警报
            personalAlarmPosition = alarmSystem.alarmPosition;
        }
        //保留当前帧的警报坐标，以备下一帧使用
        previousAlarmPosition = alarmSystem.alarmPosition;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag(GameConst.TAG_PLAYER))
            return;
        //视觉检测
        VisualInspection();
        //听觉检测
        HearingTest();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(GameConst.TAG_PLAYER))
            return;
        //离开触发器，标记为不能看到玩家
        playerInSight = false;
    }

    /// <summary>
    /// 视觉检测
    /// </summary>
    private void VisualInspection()
    {
        //默认标记不能看到玩家
        playerInSight = false;
        //求方向向量
        dir = player.position - transform.position;
        //求夹角
        float angle = Vector3.Angle(dir, transform.forward);
        //如果夹角过大，则看不到玩家
        if(angle > viewOfField/2)
            return;
        //计算眼睛的位置
        Vector3 eyesPos = transform.position + Vector3.up * GameConst.ENEMY_EYES_HEIGHT;
        //发射物理射线
        if (!Physics.Raycast(eyesPos, dir, out hit))
            return;
        //如果射线检测到的不是玩家，说明两者之间有障碍物
        if(!hit.collider.CompareTag(GameConst.TAG_PLAYER))
            return;
        //看到了玩家，触发全局警报
        alarmSystem.alarmPosition = player.position;
        //标记可以看到玩家
        playerInSight = true;
    }

    /// <summary>
    /// 听觉检测
    /// </summary>
    private void HearingTest()
    {
        //玩家是否在运动状态
        bool isLocomotion = playerAnimator.GetCurrentAnimatorStateInfo(
            0).shortNameHash == GameConst.STATE_LOCOMOTION;
        //玩家是否处于喊叫状态
        bool isShout = playerAnimator.GetCurrentAnimatorStateInfo(
            1).shortNameHash == GameConst.STATE_SHOUT;
        
        //既没有脚步声也没有喊叫声
        if(!isLocomotion && !isShout)
            return;
        
        // 计算导航到玩家位置的路径
        bool canArrive = navMeshAgent.CalculatePath(player.position, path);

        if(path.status == NavMeshPathStatus.PathPartial)
        {
            //无法到达目的地
            return;
        }
        //若无法到达目标地
        if(!canArrive)
            return;
        
        //路径点数组
        Vector3[] points = new Vector3[path.corners.Length + 2];
        
        //赋值起点和终点
        points[0] = transform.position;
        points[points.Length - 1] = player.position;
        //赋值中间的拐点
        for (int i = 1; i < points.Length - 1; i++)
        {
            points[i] = path.corners[i - 1];
        }
        
        //声明距离
        float distance = 0;

        for (int i = 0; i < points.Length - 1; i++)
        {
            //累加距离
            distance += Vector3.Distance(points[i], points[i + 1]);
        }

        //距离足够小，可以听见
        if (distance < trigger.radius)
        {
            //设置私人警报位置
            personalAlarmPosition = player.position;
        }
    }
}