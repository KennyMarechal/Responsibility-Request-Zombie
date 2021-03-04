using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersoInteract : MonoBehaviour
{
    public GameObject Obj = null;

    public GameObject Interact;

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && Obj != null)
        {
            if (Obj.transform.name == "Porte")
            {
                Obj.SendMessage("DoOuvreLaPorte");
                Obj = null;
            }
            else if (Obj.transform.name == "Barricade")
                Obj.SendMessage("DoRepare");
            else if (Obj.transform.name == "MysteryBox")
                Obj.SendMessage("DoOpenBox");
            else
                Obj.SendMessage("DoInteraction");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            Obj = collision.gameObject;
            Interact.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            if (collision.gameObject == Obj)
            {
                Obj = null;
                Interact.SetActive(false);
            }
        }
    }

}
