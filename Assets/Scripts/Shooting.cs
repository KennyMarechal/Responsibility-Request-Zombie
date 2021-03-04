using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject LaserPrefab;
    public GameObject IceBulletPrefab;
    public GameObject LightningPrefab;
    public GameObject GunPos;
    public Transform FirePoint;


    public float BulletForce = 20F;
    public float LaserForce = 40F;

    private AudioSource m_AudioSource;

    ///Reloading
    public TextMeshProUGUI MunitionsText;
    private PersoInteract m_persoInter;
    private bool isReloading = false;
    public int NbBallesChargeur ;
    public int NbBallesChargeurMax ;
    public int NbBalles ;
    public float BalleDommage;
    private float m_reloadtime;
    private float m_reloaddelay = 3f;


    [SerializeField]
    private AudioClip[] m_Sounds;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_persoInter = FindObjectOfType<PersoInteract>();
    }

    void Update()
    {
        if (GunPos.transform.childCount > 0)  {
            if (!isReloading) {
                if (Input.GetButtonDown("ShootMain") && NbBallesChargeur > 0)
                {
                    Shoot(GunPos.transform.GetChild(0).name);
                }
                if (Input.GetKeyDown(KeyCode.R) && ((NbBallesChargeur < NbBallesChargeurMax) && NbBalles > 0))
                {
                    isReloading = true;
                    m_persoInter.Interact.SetActive(true);
                    m_persoInter.Interact.GetComponent<Text>().text = "Reloading";
                    m_AudioSource.PlayOneShot(m_Sounds[4]);
                    Invoke("StopReloading", 3f);
                }
                if (!MunitionsText.gameObject.activeSelf)
                    MunitionsText.gameObject.SetActive(true);
                MunitionsText.text = string.Format("{0}/{1}:{2}", NbBallesChargeur, NbBallesChargeurMax, NbBalles);
            }
        } else if (MunitionsText.gameObject.activeSelf)
            MunitionsText.gameObject.SetActive(false);
    }

    public void StopReloading()
    {
        if (m_reloadtime + m_reloaddelay > Time.time)
            return;


        int t_NbBalles = 0; ;

        if ((NbBalles >= NbBallesChargeurMax) || NbBalles >= (NbBallesChargeurMax - NbBallesChargeur)) {
            t_NbBalles = NbBalles - (NbBallesChargeurMax - NbBallesChargeur);
        } else {
            t_NbBalles = 0;
        }

        if (NbBalles >= NbBallesChargeurMax) {
            NbBallesChargeur = NbBallesChargeurMax;
        }
        else if (NbBalles >= (NbBallesChargeurMax - NbBallesChargeur)) {
            NbBallesChargeur = NbBallesChargeur + (NbBallesChargeurMax - NbBallesChargeur);
        }
        else {
            NbBallesChargeur = NbBallesChargeur + NbBalles;
        }

        NbBalles = t_NbBalles;

        m_reloadtime = Time.time;
        isReloading = false;
        m_persoInter.Interact.GetComponent<Text>().text = "Press \"e\" to use !!";
        m_persoInter.Interact.SetActive(false);
    }

    private void Shoot(string name)
    {
        GameObject t_prefab;
        AudioClip t_audio;

        switch (name)
        {
            case "GunLaser":
            case "GunLaser(Clone)":
                t_prefab = LaserPrefab;
                t_audio = m_Sounds[1];
                break;
            case "Gun":
            case "Gun(Clone)":
                t_prefab = BulletPrefab;
                t_audio = m_Sounds[0];
                break;
            case "BlueGun":
            case "BlueGun(Clone)":
                t_prefab = IceBulletPrefab;
                t_audio = m_Sounds[1];
                break;
            case "LightningGun":
            case "LightningGun(Clone)":
                t_prefab = LightningPrefab;
                t_audio = m_Sounds[1];
                break;
            default:
                t_prefab = BulletPrefab;
                t_audio = m_Sounds[0];
                break;

        }
                       
        GameObject t_Bullet = Instantiate(t_prefab, FirePoint.position, FirePoint.rotation);

        Rigidbody2D t_Rb = t_Bullet.GetComponent<Rigidbody2D>();

        t_Rb.AddForce(FirePoint.up * BulletForce, ForceMode2D.Impulse);
        m_AudioSource.PlayOneShot(t_audio);
        NbBallesChargeur--;
    }
}
