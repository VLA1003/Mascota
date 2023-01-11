using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionAlimentosSuelo : MonoBehaviour
{
    private void OnCollisionEnter(Collision objetos)
    {
        if (objetos.gameObject.tag == "comida")
        {
            objetos.gameObject.SetActive(false);
        }
        if (objetos.gameObject.tag == "obstaculo")
        {
            objetos.gameObject.SetActive(false);
        }
    }
}
