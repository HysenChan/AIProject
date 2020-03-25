using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBots : MonoBehaviour
{
    public GameObject botPrefab;
    public int botCount;
    public GameObject target;
    //长方形“盒子”定义了随机产生的AI的初始位置
    public float minX = -10.0f;
    public float maxX = 20.0f;
    public float minZ = -10.0f;
    public float maxZ = 20.0f;
    public float Yvalue = 4.0f;

    private void Start()
    {
        Vector3 spawnPosition;
        GameObject bot;
        for (int i = 0; i < botCount; i++)
        {
            //随机选择一个生成点，实例化预制体
            spawnPosition = new Vector3(Random.Range(minX, maxX), Yvalue, Random.Range(minZ, maxZ));
            bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity) as GameObject;
            bot.GetComponent<SteeringForArrive>().target = target;
        }
    }
}
