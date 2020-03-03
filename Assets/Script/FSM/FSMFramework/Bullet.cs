using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMFramework
{
    public class Bullet : MonoBehaviour
    {
        //子弹的生命期
        public float LifeTime = 3.0f;
        //如果被子弹击中，减少的生命值
        public int damage = 50;
        //子弹出枪膛的速度
        public float beamVelocity = 100;

        public void Go()
        {
            //子弹是一个刚体，发射时，我们为它加上一个速度突变
            GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.VelocityChange);
        }

        private void FixedUpdate()
        {
            //子弹飞行过程中，速度如何变化
            GetComponent<Rigidbody>().AddForce(transform.forward * beamVelocity, ForceMode.Acceleration);
        }

        private void Start()
        {
            //一定时间后，销毁这个对象
            Destroy(gameObject, LifeTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //如果子弹与其他物体碰撞，那么销毁它
            Destroy(gameObject);
        }
    }
}
