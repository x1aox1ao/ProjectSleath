using UnityEngine;

public class GameConst
{
    #region 游戏标签

    public const string TAG_ALARMLIGHT = "AlarmLight";
    public const string TAG_MAINLIGHT = "MainLight";
    public const string TAG_SIREN = "Siren";
    public const string TAG_PLAYER = "Player";
    public const string TAG_GAMECONTROLLER = "GameController";
    public const string TAG_ENEMY = "Enemy";
    public const string TAG_INNERDOOR = "InnerDoor";
    public const string TAG_FINISH = "Finish";

    #endregion

    #region 虚拟轴、虚拟按键

    public const string AXIS_HORIZONTAL = "Horizontal";
    public const string AXIS_VERTICAL = "Vertical";
    public const string BUTTON_SNEAK = "Sneak";
    public const string BUTTON_ATTRACT = "Attract";
    public const string BUTTON_SWITCH = "Switch";

    #endregion

    #region 动画参数、状态

    public static int PARAMETER_SPEED;
    public static int PARAMETER_SNEAK;
    public static int PARAMETER_SHOUT;
    public static int PARAMETER_DOOROPEN;
    public static int PARAMETER_ANGULARSPEED;
    public static int PARAMETER_PLAYERINSIGHT;
    public static int PARAMETER_DEAD;
    public static int PARAMETER_ENDGAME;

    public static int STATE_LOCOMOTION;
    public static int STATE_SHOUT;
    public static int STATE_SHOOT;
    public static int STATE_RAISE;

    /// <summary>
    /// 静态构造
    /// </summary>
    static GameConst()
    {
        PARAMETER_SPEED = Animator.StringToHash("Speed");
        PARAMETER_SNEAK = Animator.StringToHash("Sneak");
        PARAMETER_SHOUT = Animator.StringToHash("Shout");
        PARAMETER_DOOROPEN = Animator.StringToHash("DoorOpen");
        PARAMETER_ANGULARSPEED = Animator.StringToHash("AngularSpeed");
        PARAMETER_PLAYERINSIGHT = Animator.StringToHash("PlayerInSight");
        PARAMETER_DEAD = Animator.StringToHash("Dead");
        PARAMETER_ENDGAME = Animator.StringToHash("EndGame");
        
        STATE_LOCOMOTION = Animator.StringToHash("Locomotion");
        STATE_SHOUT = PARAMETER_SHOUT;
        STATE_SHOOT = Animator.StringToHash("WeaponShoot");
        STATE_RAISE = Animator.StringToHash("WeaponRaise");
        
    }

    #endregion

    #region 游戏数据参数（Game Data Paramter）

    //俯视高度
    public const float OVERLOOKHEIGHT = 15f;
    //敌人眼睛的高度
    public const float ENEMY_EYES_HEIGHT = 1.7f;
    //玩家身体的高度
    public const float PLAYER_BODY_HEIGHT = 1.7f;

    #endregion
}
