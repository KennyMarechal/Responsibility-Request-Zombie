using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;
    public float MinX, MaxX, MinY, MaxY;

    // Update is called once per frame
    void Update()
    {
        float t_TargetX = Mathf.Clamp(Target.position.x, MinX, MaxX);
        float t_TargetY = Mathf.Clamp(Target.position.y, MinY, MaxY);

        transform.position = new Vector3(t_TargetX, t_TargetY, -10);
    }
}
