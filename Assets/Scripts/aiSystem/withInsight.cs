using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class withIngsight : Conditional
{
    public string targetTag;
    public SharedTransform target;
    public Transform[] possibleTargets;
    public float viewDistance;


    public override void OnAwake()
    {
        var targets = GameObject.FindGameObjectsWithTag(targetTag);
        possibleTargets = new Transform[targets.Length];
        for (int i = 0; i < targets.Length; ++i)
        {
            possibleTargets[i] = targets[i].transform;
        }

    }

    public override TaskStatus OnUpdate()
    {
        foreach(Transform t in possibleTargets)
        {
            if (WithinSight(t,viewDistance))
            {
                target.Value = t;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }

    public bool WithinSight(Transform targetTransform, float view)
    {
        float distance = Vector3.Distance(targetTransform.position ,transform.position); 
        return distance < view;
    }

}
