using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyController : MonoBehaviour
{
    public float Speed = 1f;
    public float HealthPoint = 2f;

    private Rigidbody2D m_Rb;
    private float m_Time = 1f;
    private Path m_Path;
    private Tuile m_CurrentTarget;
    private Animator m_Animator;
    private GameHandler m_GH;
    private Shooting m_Shooting;

    private AudioSource m_AudioSource;

    [SerializeField]
    private AudioClip[] m_Sounds;

    public Path Path
    {
        set { m_Path = value; }
    }

    public Tuile Target
    {
        get { return m_CurrentTarget; }
    }

    private void Awake()
    {
        m_Shooting = FindObjectOfType<Shooting>();
    }

    void Start()
    {
        m_GH = FindObjectOfType<GameHandler>();
        m_Rb = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
        m_CurrentTarget = m_Path?.GetNextTuile(m_CurrentTarget);
        m_Animator = GetComponent<Animator>();

        if (m_Path == null || m_CurrentTarget == null)
        {
            Debug.LogWarning("Ennemy has no possible path.");
            gameObject.SetActive(false);
            return;
        }

        //Teleporte sur le point de départ
        transform.position = m_CurrentTarget.transform.position;

        
    }

    void Update()
    {
        float t_DistanceAFaire = Speed * Time.deltaTime;

        Vector3 t_StartPoint = transform.position;
        Vector3 T_MoveToNextCheckpoint = m_CurrentTarget.transform.position - t_StartPoint;



        while (t_DistanceAFaire * t_DistanceAFaire >= T_MoveToNextCheckpoint.sqrMagnitude && m_CurrentTarget != null)
        {
            t_DistanceAFaire -= T_MoveToNextCheckpoint.magnitude;
            t_StartPoint = m_CurrentTarget.transform.position;
            m_CurrentTarget = m_Path.GetNextTuile(m_CurrentTarget);

            if (m_CurrentTarget == null)
                continue;

            T_MoveToNextCheckpoint = m_CurrentTarget.transform.position - t_StartPoint;

        }
        


        //Je suis au bout du Path
        if (m_CurrentTarget == null)
        {
            transform.position = t_StartPoint;
            //TargetReached();
        }
        else
        {
            transform.position = t_StartPoint + (m_CurrentTarget.transform.position - t_StartPoint).normalized * t_DistanceAFaire;
        }
    }

    /*private void TargetReached()
    {
        m_GH.allZombies.Remove(this.GetComponent<EnnemyController>());

        Destroy(gameObject);
    }*/

    private void OnDrawGizmos()
    {
        if (m_Path == null || m_Path.Checkpoints.Count < 2)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < m_Path.Checkpoints.Count - 1; i++)
        {
            Gizmos.DrawLine(m_Path.Checkpoints[i].transform.position, m_Path.Checkpoints[i + 1].transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Personnage"))
        {
            m_AudioSource.PlayOneShot(m_Sounds[0]);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            HealthPoint -= m_Shooting.BalleDommage;

            if (HealthPoint <= 0)
            {
                m_GH.allZombies.Remove(this.GetComponent<EnnemyController>());
                m_GH.ScorePoint += 100;
                m_GH.CurrentWaveObject.ZombieTuer();
                Destroy(gameObject);
            }
        }
    }
}
