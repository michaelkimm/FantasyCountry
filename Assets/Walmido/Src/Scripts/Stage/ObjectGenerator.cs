using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField]
    protected float innerRadius = 4f;
    [SerializeField]
    protected float outerRadius = 9f;

    [SerializeField]
    float startAngle = 0f;
    [SerializeField]
    float endAngle = 300f;

    protected Ray ray;
    protected Vector3 generatePose = Vector3.zero;

    protected virtual void GenerateObject()
    {
        generatePose = GetRandomPose(this.transform.position, innerRadius, outerRadius, startAngle, endAngle);
    }

    protected Vector3 GetRandomPose(Vector3 center, float innerRadius, float outerRadius, float startAngle_, float endAngle_)
    {
        float angle = Random.Range(startAngle_, endAngle_);
        float dist = Random.Range(innerRadius, outerRadius);

        // xøÕ z√‡ 
        Vector3 result = center + (Mathf.Cos(angle) * transform.forward + Mathf.Sin(angle) * transform.right) * dist;
        return result;
    }
}
