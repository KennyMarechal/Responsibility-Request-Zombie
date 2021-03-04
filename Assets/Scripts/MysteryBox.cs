using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    private Animator m_animator;
    private CircleCollider2D m_CircleTrigger;
    [SerializeField]
    private GameObject[] m_GunPrefabs;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_CircleTrigger = GetComponent<CircleCollider2D>();
    }

    public void OpenBox()
    {
        m_animator.SetBool("Open", true);
        m_CircleTrigger.enabled = false;
        Invoke("CloseBox", 5f);
    }

    private void CloseBox()
    {
        m_animator.SetBool("Open", false);
        
        int t_index = Random.Range(0, m_GunPrefabs.Length);
        GameObject t_gun = Instantiate(m_GunPrefabs[t_index]);
        t_gun.transform.position = transform.position;

        Invoke("BoxCanBeOpened", 150f);
    }

    private void BoxCanBeOpened()
    {
        m_CircleTrigger.enabled = true;
    }
}
