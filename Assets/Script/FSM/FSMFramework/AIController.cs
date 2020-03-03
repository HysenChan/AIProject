using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    public class AIController : AdvanceFSM
    {
        //子弹的游戏对象
        public GameObject Bullet;
        //子弹的生成点
        public Transform bulletSpawnPoint;
        //AI角色的生命值
        private int health;

        //初始化AI角色的FSM，在FSM基类的Start函数中调用
        protected override void Initialize()
        {
            //生命值设置为100
            health = 100;
            //距离上一次射击的时间
            elapsedTime = 0.0f;
            //射击速率
            shootRate = 2.0f;

            //获得敌人（这里是玩家）的transform组件
            GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
            playerTransform = objPlayer.transform;

            //如果无法获得玩家的transform组件，提示错误信息
            if (!playerTransform)
            {
                Debug.LogError("Player doesn't exist Tag,Please add one with Tag named 'Player'");
            }
            //调用ConstructFSM函数，开始构造状态机
            ConstructFSM();
        }

        //在FSM基类的Update函数中调用
        protected override void FSMUpdate()
        {
            //如果有多个事件会影响到生命值，那么可以在这里检查生命值
            //计算距离上次子弹发射后过去的时间
            elapsedTime += Time.deltaTime;
        }

        //在FSM基类的FixedUpdate函数中调用
        protected override void FSMFixedUpdate()
        {
            //调用当前状态的Reason方法，确定当前发生的转换
            CurrentState.Reason(playerTransform, transform);
            //调用当前状态的Act方法，确定角色的行为
            //注意，如果Reason中检查到满足某个转换条件，就会进行状态转换
            //因此，在这两个调用之间，CurrentState可能会发生变化
            CurrentState.Act(playerTransform, transform);
        }

        //这个方法在每个状态类的Reason方法中被调用
        public void SetTransition(Transition t)
        {
            //调用AdvanceFSM类的PerformTransition方法，设置新状态
            PerformTransition(t);
        }

        //这个函数在初始化Initialize方法中调用，为AI角色构造FSM
        private void ConstructFSM()
        {
            //找到所有便签为“巡逻点”的游戏物体
            pointList = GameObject.FindGameObjectsWithTag("PatrolPoints");

            Transform[] waypoints = new Transform[pointList.Length];
            int i = 0;

            //将pointList中的每个游戏物体的transform组件加入waypoints数组中
            foreach (GameObject obj in pointList)
            {
                waypoints[i] = obj.transform;
                i++;
            }

            //构造一个巡逻状态类
            PatrolState patrol = new PatrolState(waypoints);
            //调用巡逻状态类中的AddTransition函数，
            //将这个状态下可能的两个“转换-状态”对（“看到玩家-追逐”和“生命值-死亡”）
            //加入到PatrolState类的字典中
            patrol.AddTransiton(Transition.SawPlayer, FSMStateID.Chasing);
            patrol.AddTransiton(Transition.NoHealth, FSMStateID.Dead);

            //创建一个追逐状态类
            ChaseState chase = new ChaseState(waypoints);
            //将这个状态下可能的三个“转换-状态”对加入到ChaseState类的字典中
            chase.AddTransiton(Transition.LostPlayer, FSMStateID.Patrolling);
            chase.AddTransiton(Transition.ReachPlayer, FSMStateID.Attacking);
            chase.AddTransiton(Transition.NoHealth, FSMStateID.Dead);

            //创建一个攻击状态类
            AttackState attack = new AttackState(waypoints);
            //将这个状态下可能的三个“转换-状态”对加入到AttackState类的字典中
            attack.AddTransiton(Transition.SawPlayer, FSMStateID.Chasing);
            attack.AddTransiton(Transition.LostPlayer, FSMStateID.Patrolling);
            attack.AddTransiton(Transition.NoHealth, FSMStateID.Dead);

            //创建一个死亡状态类
            DeadState dead = new DeadState();
            //将这个状态下可能的一个“转换-状态”对加入到DeadState类的字典中
            dead.AddTransiton(Transition.NoHealth, FSMStateID.Dead);

            //调用AdvanceFSM类中的AddFSMState函数
            //将这四个状态类加入到AdvanceFSM类的fsmStates状态列表中
            AddFSMState(patrol);
            AddFSMState(chase);
            AddFSMState(attack);
            AddFSMState(dead);
        }

        //当AI角色与其他物体碰撞时，调用这个函数
        private void OnCollisionEnter(Collision collision)
        {
            //如果另一个碰撞体是子弹，说明AI角色被子弹击中
            if (collision.gameObject.tag=="Bullet")
            {
                //减少AI角色的生命值
                health -= 50;
                //如果生命值小于等于0
                if (health<=0)
                {
                    Debug.Log("Switch to Dead State");
                    //转换为死亡状态
                    SetTransition(Transition.NoHealth);
                }
            }
        }

        //发射子弹
        public void ShootBullet()
        {
            //如果距离上次发射子弹的时间大于射击速率，那么可以再次射击
            if (elapsedTime>=shootRate)
            {
                //在子弹生成位置，实例化一个子弹
                GameObject bulletObj = Instantiate(Bullet, bulletSpawnPoint.transform.position, transform.rotation) as GameObject;
                //调用bullet脚本的Go函数，子弹向前飞出
                bulletObj.GetComponent<Bullet>().Go();
                //重置流逝事件为0
                elapsedTime = 0.0f;
            }
        }
    }
}
