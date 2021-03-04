using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public List<EnnemyController> allZombies;

    //position du joueur en format de la grille

    public int ScorePoint = 0;
    public int HealthPoint = 5;
    [SerializeField]
    private TextMeshProUGUI VieText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;
    private Pathfinder m_Pf;
    [SerializeField]
    private TextMeshProUGUI m_waveText;
    [SerializeField]
    private TextMeshProUGUI m_ratioTuer;

    public EnnemySpawner[] allspawners;

    public Wave CurrentWaveObject;

    public string WaveText
    {
        set { m_waveText.text = value;  }
    }
    public string RatioZombie
    {
        set { m_ratioTuer.text = value;  }
    }


    private void Awake()
    {
        m_Pf = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        ScoreText.text = $"Score point : {ScorePoint}";
        VieText.text = $"Life point : {HealthPoint}";

        Invoke("StartGame", 5f);
    }

    private void StartGame()
    {
        WaveText = string.Format("Wave : {0}", CurrentWaveObject.CurrentWaveValue) ;
        RatioZombie = string.Format("Zombies tués: {0}/{1}", CurrentWaveObject.GetNbZombieTuerInWave, CurrentWaveObject.NbZombieWave);
        CurrentWaveObject.TrySpawn();
    }

    private void Update()
    {
        ScoreText.text = $"Score point : {ScorePoint}";
        VieText.text = $"Life point : {HealthPoint}";
    }

    public void UpdateAllZombiePath(Tuile target) {
        foreach (EnnemyController t_zombie in allZombies) {
            t_zombie.Path = m_Pf.GetPath(t_zombie.transform, target, false);
        }
    }
    
}
