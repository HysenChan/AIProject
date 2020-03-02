using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : MonoBehaviour
{
    //表示每个操控力的权重
    public float weight = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual Vector3 Force()
    {
        return new Vector3(0, 0, 0);
    }
}
