using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private GameHandler m_GH;

    public int NbzombieSimultaniously = 0;
    public int NbZombieWave = 0;
    private int NbZombieEnVie = 0;
    [SerializeField]
    private int NbZombieTuerInWave = 0;
    public int CurrentWaveValue = 1;
    public float NextWaveDelay = 0;



    [SerializeField]
    private GameObject WavePrefab;

    public int GetNbZombieTuerInWave
    {
        get { return NbZombieTuerInWave; }
    }

    private void Awake()
    {
        m_GH = FindObjectOfType<GameHandler>();
    }

    public void TrySpawn()
    {
        int t_Nb = 0;
        do {
           t_Nb = Random.Range(0, m_GH.allspawners.Length);
        } while (!m_GH.allspawners[t_Nb].IsInRangeOfPlayer);

        
        if ((NbZombieTuerInWave + NbZombieEnVie < NbZombieWave) && NbZombieEnVie < NbzombieSimultaniously) {
            m_GH.allspawners[t_Nb].Spawn(CurrentWaveValue);
            NbZombieEnVie++;
        }

        if (NbZombieTuerInWave < NbZombieWave)
            Invoke("TrySpawn", 1f);
        else
            Invoke("LoadNextWave", NextWaveDelay);
    }

    private void LoadNextWave()
    {
        Destroy(gameObject);
        Wave t_NextWave = Instantiate(WavePrefab,transform.parent).GetComponent<Wave>();
        m_GH.CurrentWaveObject = t_NextWave;
        int t_index = CurrentWaveValue + 1;
        t_NextWave.CurrentWaveValue = t_index;
        t_NextWave.NbZombieWave = t_index * 10;
        t_NextWave.NextWaveDelay = NextWaveDelay + 2;
        t_NextWave.NbZombieTuerInWave = 0;
        m_GH.WaveText = string.Format("Wave : {0}", t_index);
        m_GH.RatioZombie = string.Format("Zombies tués: {0}/{1}", t_NextWave.NbZombieTuerInWave, t_NextWave.NbZombieWave);
        t_NextWave.TrySpawn();
    }

    public void ZombieTuer()
    {
        NbZombieTuerInWave++;
        NbZombieEnVie--;
        m_GH.RatioZombie = string.Format("Zombies tués: {0}/{1}", NbZombieTuerInWave, NbZombieWave);
    }
}
