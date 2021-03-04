using Completed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetRandom : MonoBehaviour
{
    public Transform Target;
    private float m_MinX, m_MaxX, m_MinY, m_MaxY;

    private void Start()
    {
        float t_ratioX = 8.5f;
        float t_ratioY = 4.5f;
        float t_Columns = FindObjectOfType<BoardManager>().columns;
        float t_Rows = FindObjectOfType<BoardManager>().rows;
        m_MaxX = t_Columns - t_ratioX;
        m_MaxY = t_Rows - t_ratioY;
        m_MinX = -0.5f;
        m_MinY = -0.5f;
    }

    // Update is called once per frame
    void Update()
    {
       
        float t_TargetX = Mathf.Clamp(Target.position.x, m_MinX, m_MaxX);
        float t_TargetY = Mathf.Clamp(Target.position.y, m_MinY, m_MaxY);

        transform.position = new Vector3(t_TargetX, t_TargetY, -10);
    }
}
