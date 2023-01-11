using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinijuegoAlimentacion : MonoBehaviour
{
    // Creaci�n de variables e instancia.
    public static MinijuegoAlimentacion _instance;
    public static MinijuegoAlimentacion Instance { get { return _instance; } }

    public float moviemientoEjeX;
    float moviemientoEjeY;
    float moviemientoEjeZ;
    public float velocidadMovimientoMascota = 2.5f;
    public float velocidadMovimientoSpawner = 1.5f;
    public GameObject mascota;
    public GameObject comida;
    public GameObject obstaculo;
    public GameObject suelo;
    private int contadorComida;
    private int contadorOstaculos;
    public GameObject camaraMinijuego;
    public GameObject camaraPrincipal;
    public bool jugando = false;
    public int tiempoJugando = 12;
    public float tiempoJugandoTotal = 0;

    private void Awake()
    {
        // Llamada de la instancia
        if (Instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // Reseteo de la posici�n de la mascota.
        mascota.transform.position = new Vector3(0, 0, 0);
        jugando = true;
        tiempoJugandoTotal = 0;
    }

    private void OnEnable()
    {
        // Activaci�n de la c�mara, los spawner y la cantidad de alimentos y obst�culos que spawnean.
        camaraMinijuego.SetActive(true);
        camaraPrincipal.SetActive(false);
        contadorComida = 4;
        contadorOstaculos = 3;
        InvokeRepeating("SpawnComida", 1, 2);
        InvokeRepeating("SpawnObstaculo", 2, 2);
    }

    void Update()
    {
        //Movimiento del personaje.
        moviemientoEjeX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidadMovimientoMascota;
        mascota.transform.Translate(moviemientoEjeX, moviemientoEjeY, moviemientoEjeZ);
        tiempoJugandoTotal += Time.deltaTime;
        if (tiempoJugandoTotal >= tiempoJugando)
        {
            gameObject.SetActive(false);
            jugando = false;
        }
    }

    // Funci�n para controlar el spawn de la comida.
    void SpawnComida()
    {
        var posicionAleatoriaComida = new Vector3(Random.Range(-3, 3), 6, 0);
        Instantiate(comida, posicionAleatoriaComida, transform.rotation * Quaternion.Euler(90f, 90f, 90f));
        if (--contadorComida == 0)
        {
            CancelInvoke("SpawnComida");
        }
    }

    // Funci�n para controlar el spawn de los obst�culos.
    void SpawnObstaculo()
    {
        var posicionAleatoriaObstaculo = new Vector3(Random.Range(-3, 3), 6, 0);
        Instantiate(obstaculo, posicionAleatoriaObstaculo, transform.rotation * Quaternion.Euler(180f, 0f, 0f));
        if (--contadorOstaculos == 0)
        {
            CancelInvoke("SpawnObstaculo");
        }
    }

    // Desactivaci�n de la c�mara y reseteo de la posici�n de la mascota tras acabar el minijuego.
    private void OnDisable()
    {
        camaraMinijuego.SetActive(false);
        camaraPrincipal.SetActive(true);
        mascota.transform.position = new Vector3(0, 0, 0);
    }
}
