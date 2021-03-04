using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyLooks : MonoBehaviour
{

    public Tuile m_CurrentTarget;
    private PersoController m_persoController;

    public Rigidbody2D ttt;
    public float t_Angle;

    public Vector2 t_LookDirection;

    private void Awake()
    {
        m_persoController = FindObjectOfType<PersoController>();
    }

    private void Start()
    {
        m_CurrentTarget = m_persoController.PlayerLastTuile;
    }

    private void Update()
    {
        m_CurrentTarget = m_persoController.PlayerLastTuile;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        t_LookDirection = m_CurrentTarget.transform.position - transform.position;
        t_LookDirection = t_LookDirection.normalized;

        Quaternion rotation = new Quaternion(t_LookDirection.x, t_LookDirection.y, 0, 0);
        transform.rotation = rotation;
    }
}
