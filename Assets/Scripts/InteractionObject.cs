using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    private Grille m_Grille;

    private GameObject GunPos;
    private GameObject m_perso;

    private AudioSource m_AudioSource;

    private Shooting m_Shooting;
    

    [SerializeField]
    private AudioClip[] m_Sounds;

    [SerializeField]
    private Sprite Chemin;

    

    private float lastHealedTime;
    private float healingDelay = 1f;

    private void Start()
    {
        m_Grille = FindObjectOfType<Grille>();
        m_AudioSource = GetComponent<AudioSource>();
        m_perso = FindObjectOfType<PersoController>().gameObject;
        GunPos = m_perso.transform.GetChild(0).gameObject;
        m_Shooting = m_perso.GetComponent<Shooting>();   
    }
    public void DoInteraction()
    {
        if (GunPos.transform.childCount == 0)
        {
            gameObject.transform.parent = GunPos.transform;
            gameObject.transform.position = GunPos.transform.position;
            gameObject.transform.rotation = GunPos.transform.rotation;


            gameObject.tag = "PorteOuvert";
            GunPos.gameObject.GetComponentInParent<PersoInteract>().Interact.SetActive(false);
            Destroy(gameObject.GetComponent<InteractionObject>());


            switch (gameObject.name)
            {
                case "Gun":
                case "Gun(Clone)":
                    m_Shooting.NbBalles = 9999;
                    m_Shooting.NbBallesChargeur = 30;
                    m_Shooting.NbBallesChargeurMax = 30;
                    m_Shooting.BalleDommage = 0.5f;
                    break;
                case "BlueGun":
                case "BlueGun(Clone)":
                    m_Shooting.NbBalles = 180;
                    m_Shooting.NbBallesChargeur = 15;
                    m_Shooting.NbBallesChargeurMax = 15;
                    m_Shooting.BalleDommage = 2f;
                    break;
                case "GunLaser":
                case "GunLaser(Clone)":
                    m_Shooting.NbBalles = 250;
                    m_Shooting.NbBallesChargeur = 25;
                    m_Shooting.NbBallesChargeurMax = 25;
                    m_Shooting.BalleDommage = 1000f;
                    break;
                case "LightningGun":
                case "LightningGun(Clone)":
                    m_Shooting.NbBalles = 180;
                    m_Shooting.NbBallesChargeur = 15;
                    m_Shooting.NbBallesChargeurMax = 15;
                    m_Shooting.BalleDommage = 5f;
                    break;
            }
            m_Shooting.FirePoint = gameObject.transform.GetChild(0).GetComponentInChildren<Transform>();
        }    
        else
        {
            //Detruire l'arme
            Destroy(GunPos.transform.GetChild(0).gameObject);
            Invoke("DoInteraction", 0f);
        } 
    }

    public void DoOuvreLaPorte()
    {
        Tuile t_tuile = GetComponent<Tuile>();

        OuvrirPorte(gameObject);

        OuvrePorteVoisine(t_tuile);

        m_AudioSource.PlayOneShot(m_Sounds[0]);

        Destroy(this);
    }
    public void DoRepare()
    {
        if (Time.time < lastHealedTime + healingDelay)
            return;
        lastHealedTime = Time.time;
        if (!transform.GetChild(0).gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(true);
        GetComponentInChildren<BreakingDoorController>().HealBreakingDoor();
    }
    public void DoOpenBox()
    {
      GetComponent<MysteryBox>().OpenBox();
    }

    private void OuvrePorteVoisine(Tuile t_tuile)
    {
        for (int i = (int)(t_tuile.y - 1); i <= (t_tuile.y + 1); i++)
        {
            GameObject t_nextTuile = m_Grille.Tuiles[t_tuile.x, i].gameObject;
            if (t_nextTuile.transform.name == "Porte")
            {
                OuvrirPorte(t_nextTuile);
                Destroy(t_nextTuile.GetComponent<InteractionObject>());
            }
            i++;
        }
        for (int i = (int)(t_tuile.x - 1); i <= (t_tuile.x + 1); i++)
        {
            GameObject t_nextTuile = m_Grille.Tuiles[i, t_tuile.y].gameObject;
            if (t_nextTuile.transform.name == "Porte")
            {
                OuvrirPorte(t_nextTuile);
                Destroy(t_nextTuile.GetComponent<InteractionObject>());
            }
            i++;
        }
    }

    private void OuvrirPorte(GameObject t_porte)
    {
        t_porte.GetComponent<SpriteRenderer>().sprite = Chemin;
        t_porte.GetComponent<Tuile>().BaseCost = 1;
        t_porte.tag = "PorteOuvert";
        Destroy(t_porte.GetComponent<BoxCollider2D>());
        GunPos.gameObject.GetComponentInParent<PersoInteract>().Interact.SetActive(false);
    }
}
