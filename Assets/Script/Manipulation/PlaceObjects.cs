using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    public GameObject objectsToPlace;
    public int count;

    //海鸥的初始位置在一个半径为radius的球体内随机产生
    public float radius;
    public bool isPlanar;

    private void Awake()
    {
        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i < count; i++)
        {
            position = transform.position + Random.insideUnitSphere * radius;
            if (isPlanar)
            {
                position.y = objectsToPlace.transform.position.y;
            }
            //实例化海鸥的预制体
            Instantiate(objectsToPlace, position, Quaternion.identity);
        }
    }
}
