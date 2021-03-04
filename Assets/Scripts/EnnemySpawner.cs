using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
    public GameObject PREFAB_Ennemy;
    public Tuile TargetTuile;
    private PersoController m_playerController;
    private GameHandler m_GH;
    public bool IsInRangeOfPlayer = false;
    Pathfinder m_pathf;


    private void Awake()
    {
        if(PREFAB_Ennemy.GetComponent<EnnemyController>() == null)
        {
            Debug.LogError("EnnemySpawner is spawning an object without an EnnemyController.");
            gameObject.SetActive(false);
        }
        m_playerController = FindObjectOfType<PersoController>();
        m_GH = FindObjectOfType<GameHandler>();
        m_pathf = FindObjectOfType<Pathfinder>();

    }

    private void Update()
    {
        TargetTuile = m_playerController.PlayerLastTuile;
    }

    public void Spawn(int t_vieZombie)
    {
        GameObject t_Ennemy = Instantiate(PREFAB_Ennemy);
        EnnemyController t_EnnemyController = t_Ennemy.GetComponent<EnnemyController>();
        t_EnnemyController.HealthPoint = t_vieZombie;
;
        t_EnnemyController.Path = m_pathf.GetPath(this.transform, TargetTuile, false);

        m_GH.allZombies.Add(t_Ennemy.GetComponent<EnnemyController>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Joueur"))
        {
            IsInRangeOfPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Joueur"))
        {
            IsInRangeOfPlayer = true;
        }
    }
}
