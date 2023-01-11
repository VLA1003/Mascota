using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PruebaGuardado : MonoBehaviour
{
    [SerializeField]
    Slider pruebaSlider;
    [SerializeField]
    TMP_InputField pruebaInputField;

    private void Awake()
    {
        pruebaSlider.value = PlayerPrefs.GetFloat("volumen", 2);
        pruebaInputField.text = PlayerPrefs.GetString("nombre", "Jugador");
    }

    public void Borrar()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Guardar()
    {
        PlayerPrefs.SetFloat("volumen", pruebaSlider.value);
        PlayerPrefs.SetString("nombre", pruebaInputField.text);
        PlayerPrefs.Save();
    }
}
