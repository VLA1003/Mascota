using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionAlimentos : MonoBehaviour
{
    private void OnCollisionEnter(Collision objetos)
    {
        if (objetos.gameObject.tag == "comida")
        {
            objetos.gameObject.SetActive(false);
            PruebaControladorTiempo.Instance.SumarPuntosAlimento();
        }
        if (objetos.gameObject.tag == "obstaculo")
        {
            objetos.gameObject.SetActive(false);
            PruebaControladorTiempo.Instance.RestarPuntosObstaculos();
        }
    }
}
