using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    //这个AI角色包含的操控行为列表
    private Steering[] steerings;

    //设置这个AI角色能达到的最大速度
    public float maxSpeed = 10;

    //设置能施加到这个AI角色的力的最大值
    private float maxForce=100;

    //最大速度的平方，通过预先算出并存储，节省资源
    protected float sqrMaxSpeed;

    //AI角色的质量
    public float mass = 1;

    //AI角色的速度
    public Vector3 velocity;

    //控制转向时的速度
    public float damping = 0.9f;

    //操控力的计算间隔时间，为了达到更高的帧率，操控力不需要每帧更新
    public float computeInterval = 0.2f;

    //是否在二维平面上，如果是，计算两个GameObject的距离时，忽略y值的不同；
    public bool isPlanar = true;

    //计算得到的操控力
    private Vector3 steeringForce;

    //AI角色的加速度
    protected Vector3 acceleration;

    //计时器
    private float timer;

    protected void Start()
    {
        steeringForce = new Vector3(0, 0, 0);
        sqrMaxSpeed = maxSpeed * maxSpeed;
        timer = 0;
        //获得这个AI角色所包含的操控行为列表
        steerings = GetComponents<Steering>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        steeringForce = new Vector3(0, 0, 0);
        //如果距离上次计算操控力的时间大于设定的时间间隔computeInterval；
        //再次计算操控力；
        if (timer>computeInterval)
        {
            //将操控行为列表中的所有操控行为对应的操控力进行带权重的求和；
            foreach (Steering s in steerings)
            {
                if (s.enabled)
                {
                    steeringForce += s.Force() * s.weight;
                }
            }

            //使操控力不大于maxForce；
            steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
            //力除以质量，求出加速度；a=F/m
            acceleration = steeringForce / mass;
            //重新从0开始计时；
            timer = 0;
        }
    }
}
