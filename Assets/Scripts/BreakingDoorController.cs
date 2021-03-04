using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingDoorController : MonoBehaviour
{
    private float lastHitTime = -0.5f;
    private float immunetime = 0.5f;
    public float Health = 5;
    private float m_maxhealth = 5;

    [SerializeField]
    private LayerMask m_EnemyLayers;

    private Animator m_animator;

    private AudioSource m_AudioSource;

    [SerializeField]
    private AudioClip[] m_Sounds;

    
    private void Awake()
    {
        m_animator = gameObject.transform.GetComponentInParent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_animator.SetFloat("Health", Health/m_maxhealth);

        if (Health == 0)
            gameObject.SetActive(false);
        else if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lastHitTime + immunetime > Time.time) return;

        if (m_EnemyLayers == (m_EnemyLayers | 1 << collision.gameObject.layer))
        {
            lastHitTime = Time.time;
            takeDamage(1);
        }
    }
        
    private void takeDamage(int dommage)
    {
        if (dommage < 0)
            return;

        float updateHealth = Health - dommage;
        Health = updateHealth <= 0 ? 0 : updateHealth;

        GetComponentInParent<Tuile>().BaseCost = (uint)(1 + Health);

        m_AudioSource.PlayOneShot(m_Sounds[0]);
    }

    public void HealBreakingDoor()
    {
        float updateHealth = Health + 1;
        Health = updateHealth >= m_maxhealth ? m_maxhealth : updateHealth;

        GetComponentInParent<Tuile>().BaseCost = (uint)(1 + Health);

        m_AudioSource.PlayOneShot(m_Sounds[0]);
    }
}
