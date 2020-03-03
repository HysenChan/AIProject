using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    public abstract class FSMState : MonoBehaviour
    {
        //字典，字典中的每一项都记录了一个“转换-状态”对信息
        protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();

        //状态编号ID
        protected FSMStateID stateID;
        public FSMStateID ID
        {
            get { return this.stateID; }
        }

        //下面是需要用到的，与各状态相关的变量
        //目标点的位置
        protected Vector3 destPos;

        //巡逻点的数组，其中存储了巡逻时需要经过的点
        protected Transform[] wayPoints;

        //转向的速度
        protected float curRotSpeed;

        //移动的速度
        protected float curSpeed;

        //AI角色与玩家的距离小于这个值时，开始追逐
        protected float chaseDistance = 40.0f;
        //AI角色与玩家的距离小于这个值时，开始攻击
        protected float attackDistance = 20.0f;
        //在巡逻过程中，如果AI角色与某个巡逻点的距离小于这个值，认为已经到达了这个点
        protected float arriveDistance = 3.0f;

        //向字典中添加项；每项是一个“转换-状态”对
        public void AddTransiton(Transition transition,FSMStateID id)
        {
            //检查这个转换（可以看做是字典的关键字）是否已经在字典中
            if (map.ContainsKey(transition))
            {
                //如果已经在字典中，输出信息
                //这是因为在确定有限状态机中，一个转换只能对应一个新状态
                Debug.LogWarning("FSMState ERROR:transition is already inside the map!");
                return;
            }
            //如果不在字典中，那么将这个转换和转换后的状态作为一个新的字典项
            //加入字典
            map.Add(transition, id);
            Debug.Log("Added:" + transition + " with id:" + id);
        }

        //从字典中删除一项
        public void DeleteTransition(Transition transition,FSMStateID id)
        {
            //检查这项是否在字典中，如果在，则移除
            if (map.ContainsKey(transition))
            {
                map.Remove(transition);
                return;
            }
            //如果要删除的项不在字典中，则报告错误
            Debug.LogWarning("FSMState Error:Transition passed was not on this State's List");
        }

        //通过查询字典，确定在当前状态下，发生trans转换时，应该转换到的新状态编号；
        //并返回这个新状态编号
        public FSMStateID GetOutputState(Transition trans)
        {
            return map[trans];
        }

        //Reason方法用来确定是否需要转换到其他状态，应该发生哪个转换
        public abstract void Reason(Transform player, Transform npc);

        //Act方法定义了在本状态的角色行为，例如移动，动画等
        public abstract void Act(Transform player, Transform npc);

        //随机的从巡逻点数组中选择一个点，将这个点设置为目标点
        public void FindNextPoint()
        {
            int rndIndex = Random.Range(0, wayPoints.Length);
            Vector3 rndPosition = Vector3.zero;
            destPos = wayPoints[rndIndex].position + rndPosition;
        }
    }
}
