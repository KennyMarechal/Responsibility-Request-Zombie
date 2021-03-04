using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersoController : MonoBehaviour
{
    public float Speed ;
    public Rigidbody2D Rb;
    public GameObject EndPanel;

    private Vector2 m_Movement;
    private Vector2 m_MousePos;

    private Animator m_Animation;
    

    public Camera Cam;

    public Tuile PlayerLastTuile;
    public TextMeshProUGUI VieText;
    public TextMeshProUGUI ScoreText;
    private Vector2Int m_playerLastTuile;
    private Grille m_grille;
    private GameHandler m_GH;

    private void Awake()
    {
        m_Animation = gameObject.transform.GetComponentInChildren<Animator>();
        m_grille = FindObjectOfType<Grille>();
    }

    private void Start()
    {
        Vector2Int t_currentTile = m_grille.WorldToGrid(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        m_playerLastTuile = t_currentTile;
        PlayerLastTuile = m_grille.GetTuile(t_currentTile);
        m_GH = FindObjectOfType<GameHandler>();
        EndPanel.SetActive(false);
        Invoke("MyUpdate", 0f);
    }

    void Update()
    {
        m_Movement.x = Input.GetAxisRaw("Horizontal");
        m_Movement.y = Input.GetAxisRaw("Vertical");

        if (m_Movement.x != 0 || m_Movement.y != 0)
            Speed = 5f;
        else
            Speed = 0;
        
        m_MousePos = Cam.ScreenToWorldPoint(Input.mousePosition);


        m_Animation.SetFloat("Speed", Mathf.Abs(Speed)/5f);

    }
    void FixedUpdate()
    {
        Rb.MovePosition(Rb.position + m_Movement * Speed * Time.fixedDeltaTime);

        Vector2 t_LookDirection = m_MousePos - Rb.position;

        float t_Angle = Mathf.Atan2(t_LookDirection.y, t_LookDirection.x) * Mathf.Rad2Deg - 90f;

        Rb.rotation = t_Angle;
    }

    void MyUpdate()
    {
        Vector2Int t_currentTile = m_grille.WorldToGrid(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        if (t_currentTile != m_playerLastTuile)
        {
            m_playerLastTuile = t_currentTile;
            PlayerLastTuile = m_grille.GetTuile(t_currentTile);
            m_GH.UpdateAllZombiePath(PlayerLastTuile);
        }
        Invoke("MyUpdate", 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (m_GH.HealthPoint <= 1)
            {
                Destroy(gameObject);
                Time.timeScale = 0;
                EndPanel.SetActive(true);

            }
            else
            {
                m_GH.HealthPoint--;
            }
        }
    }
}
