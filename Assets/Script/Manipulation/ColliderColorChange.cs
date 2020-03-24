using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderColorChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //如果与其他碰撞体碰撞，那么碰撞体变成红色
        Debug.Log("collide0");
        if (other.gameObject.GetComponent<Vehicle>()!=null)
        {
            Debug.Log("Collide!");
            Material material = GetComponent<Renderer>().material;
            material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //碰撞体变回灰色
        Material material = GetComponent<Renderer>().material;
        material.color = Color.gray;
    }
}
