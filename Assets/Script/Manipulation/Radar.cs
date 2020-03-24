using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    //碰撞体的数组
    private Collider[] colliders;
    //计时器
    private float timer = 0;
    //邻居列表
    public List<GameObject> neighbors;
    //无需每帧进行检测，该变量设置检测的时间间隔
    public float checkInterval = 0.3f;
    //设置邻域半径
    public float detectRadius = 10f;
    //设置检测哪一层的游戏对象
    public LayerMask layersChecked;

    private void Start()
    {
        //初始化邻居列表
        neighbors = new List<GameObject>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //如果距离上次检测的时间大于所设置的检测时间间隔，那么再次检测
        if (timer>checkInterval)
        {
            //清除邻居列表
            neighbors.Clear();
            //查找当前AI角色领域内的所有碰撞体
            colliders = Physics.OverlapSphere(transform.position, detectRadius, layersChecked);
            //对于每个检测到的碰撞体，获取Vehicle组件，并且加入邻居列表中
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Vehicle>())
                {
                    neighbors.Add(colliders[i].gameObject);
                }
            }
            //计时器归0
            timer = 0;
        }
    }
}
