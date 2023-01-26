using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [Header("平滑过渡时间")] 
    [Range(0.1f,1)]
    public float dampTime = 0.2f;
    [Header("死角度数")]
    public float deadZone = 5f;
    
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemySighting enemySighting;
    
    //角色的速度与角速度
    private float speed, angularSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemySighting = GetComponent<EnemySighting>();
        player = GameObject.FindWithTag(GameConst.TAG_PLAYER).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        //玩家活着
        if (playerHealth.hp > 0)
        {
            //设置能否看到玩家到动画参数
            animator.SetBool(GameConst.PARAMETER_PLAYERINSIGHT,
                enemySighting.playerInSight);
        }
        else
        {
            //设置为看不到玩家
            animator.SetBool(GameConst.PARAMETER_PLAYERINSIGHT, false);
        }
        //计算当前角色的期望速度向量在机器人自身前方方向的投影向量
        Vector3 projection = Vector3.Project(
            navMeshAgent.desiredVelocity,
            transform.forward);
        //将投影向量的模，作为直线速度的动画参数
        speed = projection.magnitude;

        //计算当前角色的期望速度向量与机器人自身前方方向的夹角
        float angle = Vector3.Angle(transform.forward,
            navMeshAgent.desiredVelocity);
        //求当前角色的期望速度向量与机器人自身前方方向的叉乘
        Vector3 normal = Vector3.Cross(transform.forward,
            navMeshAgent.desiredVelocity);

        //期望速度为0
        if (navMeshAgent.desiredVelocity == Vector3.zero)
        {
            angle = 0;
        }

        if (angle < deadZone && enemySighting.playerInSight)
        {
            //设置角度为零，停止使用动画的方式进行旋转
            angle = 0;
            //直接看向期望速度方向
            transform.LookAt(player);
        }
        
        //法向量朝下，期望速度在左边
        if (normal.y < 0)
        {
            //对应的角速度也是朝左边，即为负值
            angle *= -1;
        }
        
        //将角度转换为弧度
        angle *= Mathf.Deg2Rad;
        
        //获取角速度
        angularSpeed = angle;
        
        //设置动画参数
        animator.SetFloat(GameConst.PARAMETER_SPEED,
            speed,dampTime,Time.deltaTime);
        animator.SetFloat(GameConst.PARAMETER_ANGULARSPEED,
            angularSpeed,dampTime,Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        //通过动画的单位时间位移，除以单位时间，得到瞬时速度
        navMeshAgent.velocity = animator.deltaPosition / Time.deltaTime;
        //设置旋转
        transform.rotation = animator.rootRotation;
    }
}