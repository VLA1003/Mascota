using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PruebaControladorTiempo : MonoBehaviour
{
    // Creación de variables e instancia.
    public static PruebaControladorTiempo _instance;
    public static PruebaControladorTiempo Instance { get { return _instance; } }
    
    string stringHoraHambre;
    string stringHoraRestadoPuntosHambre;
    string stringHoraCaricias;
    string stringHoraRestadoPuntosCaricia;
    bool acariciando = false;
    public int tiempoAcariciando = 2;
    public float tiempoAcariciandoTotal = 0;
    public ParticleSystem corazones;
    public ParticleSystem luzCorazones;
    public GameObject pantallaMuerte;
    public GameObject pantallaPrincipal;

    [SerializeField]
    public Slider barraPuntos;

    [SerializeField]
    public int puntosAmor = 100;

    [SerializeField]
    public int tiempoParaTenerHambre = 10800;
    public int tiempoParaCaricias = 86400;
    public int frecuenciaRestadoPuntos = 1;
    public int puntosSumados = 10;
    public int puntosRestados = 10;

    [SerializeField]
    GameObject mascota;

    [SerializeField]
    GameObject mascotaBebe, mascotaJunior, mascotaSenior, mascotaRey;

    
    private void Awake()
    {
        // Llamada a la instancia.
        if (Instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        puntosAmor = PlayerPrefs.GetInt("puntosAmor", 100);
        mascota.transform.localScale = mascota.transform.localScale * PlayerPrefs.GetFloat("gordura", 1f);
    }


    void Start()
    {
        // Mensaje que indica el tiempo actual.
        Debug.Log("Ahora mismo = " + DateTime.Now.ToString());

        //Para probar el funcionamiento lo primero que hago
        //es calcular cuando va a tener hambre. Para simplificarlo, le digo que será en un valor determinado (tiempoCuandoTendraHambre), siendo
        //en este caso 3 horas.
        DateTime cuandoTendraHambre = DateTime.Now.AddSeconds(tiempoParaTenerHambre);

        //Almaceno en un string la hora de cuando tendrá hambre.
        stringHoraHambre = PlayerPrefs.GetString("horaAlimentado");
        Debug.Log("Tendrá hambre a las " + stringHoraHambre);
        stringHoraRestadoPuntosHambre = DateTime.Now.ToString();
    }
    void Update()
    {
        // Funciones para determinar el estado de la mascota (mascota muerta, bebé, junior, senior o rey)
        // dependiendo de los puntos de amor que tenga. Si la mascota muere, se borran los datos.
        if (puntosAmor <= 0)
        {
            mascotaBebe.SetActive(false);
            mascotaJunior.SetActive(false);
            mascotaSenior.SetActive(false);
            mascotaRey.SetActive(false);
            BorrarDatos();
            pantallaMuerte.SetActive(true);
            pantallaPrincipal.SetActive(false);
        }
        
        if (puntosAmor >= 1 && puntosAmor <= 5)
        {
            mascotaBebe.SetActive(true);
            mascotaJunior.SetActive(false);
            mascotaSenior.SetActive(false);
            mascotaRey.SetActive(false);
        }

        if (puntosAmor >= 6 && puntosAmor <= 20)
        {
            mascotaBebe.SetActive(false);
            mascotaJunior.SetActive(true);
            mascotaSenior.SetActive(false);
            mascotaRey.SetActive(false);
        }

        if (puntosAmor >= 21 && puntosAmor <= 60)
        {
            mascotaBebe.SetActive(false);
            mascotaJunior.SetActive(false);
            mascotaSenior.SetActive(true);
            mascotaRey.SetActive(false);
        }

        if (puntosAmor >= 61 && puntosAmor <= 100)
        {
            mascotaBebe.SetActive(false);
            mascotaJunior.SetActive(false);
            mascotaSenior.SetActive(false);
            mascotaRey.SetActive(true);
        }

        // Funciones para determinar el tiempo que se está acariciando a la mascota y soltar partículas de corazones en el caso de que
        // se le acaricie.
        if (Input.GetMouseButtonDown(0))
        {
            acariciando = true;
            tiempoAcariciandoTotal = 0;
        }

        if (acariciando && Input.GetMouseButton(0))
        {
            tiempoAcariciandoTotal += Time.deltaTime;

            if (tiempoAcariciandoTotal >= tiempoAcariciando)
            {
                Acariciar();
                acariciando = false;
                corazones.Play();
                luzCorazones.Play();
            }
        }

        // Sistema para asociar los puntos de amor al llenado de la barra de vida de la mascota.
        barraPuntos.value = puntosAmor;

        //Carga desde un string (podría ser un string sacado desde PlayerPrefs...) la fecha (con hora, mes y días).
        //DateTime.Parse transforma una cadena de texto (string) en una fecha.

        DateTime cuandoTendraHambre = DateTime.Parse(stringHoraHambre);
        DateTime ultimaVezRestado = DateTime.Parse(stringHoraRestadoPuntosHambre);

        //Comparo la fecha de cuando tendrá hambre con la actual.
        //En caso de haberse pasado la hora de comer, se mostrará el mensaje.
        if (TieneHambre() && PuedePerderPuntos())
        {
            Debug.Log("¡Tengo hambre!");
            puntosAmor -= puntosRestados;
            PlayerPrefs.SetInt("puntosAmor", puntosAmor);
            stringHoraRestadoPuntosHambre = DateTime.Now.ToString();
        }
    }

    // Variable booleana que nos indica si la mascota tiene hambre.
    public bool TieneHambre()
    {
        DateTime cuandoTendraHambre = DateTime.Parse(stringHoraHambre);
        return cuandoTendraHambre < DateTime.Now;
    }

    // Variable booleana que nos indica si la mascota puede perder puntos.
    public bool PuedePerderPuntos()
    {
        DateTime ultimaVezRestado = DateTime.Parse(stringHoraRestadoPuntosHambre);
        return ultimaVezRestado.AddSeconds(frecuenciaRestadoPuntos) < DateTime.Now;
    }

    public void Alimentar()
    {
        if (TieneHambre())
        {
            DateTime cuandoTendraHambre = DateTime.Now.AddSeconds(tiempoParaTenerHambre);
            stringHoraHambre = cuandoTendraHambre.ToString();
            PlayerPrefs.SetString("horaAlimentado", stringHoraHambre);
            Debug.Log("Tendrá hambre a las " + stringHoraHambre);
            PlayerPrefs.SetInt("puntosAmor", puntosAmor);
        }
        else
        {
            Debug.Log("¡No puedo comer más!");
            Vector3 nuevoTamaño = mascota.transform.localScale;
            nuevoTamaño.x *= 1.1f;
            mascota.transform.localScale = nuevoTamaño;
        }
        
        PlayerPrefs.Save();
    }

    // Función para sumar puntos al lanzar un rayo a la mascota.
    public void Acariciar()
    {
        Vector3 posicionMascota = Input.mousePosition;
        Ray rayoMascota = Camera.main.ScreenPointToRay(posicionMascota);
        RaycastHit infoMascota;
        if (Physics.Raycast(rayoMascota, out infoMascota) == true)
        {
            if (infoMascota.collider.tag.Equals("mascota"))
            {
                puntosAmor += 10;
                PlayerPrefs.SetInt("puntosAmor", puntosAmor);
            }
        }

    }

    // Función para sumar puntos en el minijuego de alimentación al recoger alimentos. Cuando se hace esto, se guardan los puntos.
    public void SumarPuntosAlimento()
    {
        puntosAmor += 3;
        PlayerPrefs.SetInt("puntosAmor", puntosAmor);
    }

    // Función para restar puntos en el minijuego de alimentación al impactar con obstáculos. Cuando se hace esto, se guardan los puntos.
    public void RestarPuntosObstaculos()
    {
        puntosAmor -= 1;
        PlayerPrefs.SetInt("puntosAmor", puntosAmor);
    }

    // Función para borrar los datos de la partida. Al hacerlo, se sobreescriben los datos con los datos por defecto.
    public void BorrarDatos()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    // Función para salir del juego.
    public void Salir()
    {
        Application.Quit();
    }
}
