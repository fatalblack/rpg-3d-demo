using UnityEngine;

public class LuzVela : MonoBehaviour
{
    // variables públicas
    public int rangoMinimo = 2;
    public int rangoMaximo = 4;
    public int intensidadMinima = 2;
    public int intensidadMaxima = 4;
    public int cambiarCadaXSegundos = 3;

    // variables privadas
    float rangoInicio = 0;
    float rangoFin = 0;
    float intensidadInicio = 0;
    float intensidadFin = 0;
    Light _luz;

    // Start is called before the first frame update
    void Start()
    {
        _luz = GetComponent<Light>();

        // generamos la luz cada X segundos (cambiarCadaXSegundos)
        InvokeRepeating(nameof(GenerarLuzAleatoria), 0, cambiarCadaXSegundos);
    }

    void GenerarLuzAleatoria()
	{
        // si rangoInicio está en 0 significa que es la primera vez que iluminará la vela por lo cual ponemos como valor el mínimo
        rangoInicio = rangoInicio == 0 ? rangoMinimo : rangoInicio;
        // si rangoFin está en 0 significa que es la primera vez que iluminará la vela por lo cual ponemos como valor el máximo
        rangoFin = rangoFin == 0 ? rangoMaximo : rangoFin;

        // si intensidadInicio está en 0 significa que es la primera vez que iluminará la vela por lo cual ponemos como valor el mínimo
        intensidadInicio = intensidadInicio == 0 ? intensidadMinima : intensidadInicio;
        // si intensidadFin está en 0 significa que es la primera vez que iluminará la vela por lo cual ponemos como valor el máximo
        intensidadFin = intensidadFin == 0 ? intensidadMaxima : intensidadFin;

        // si rangoInicio es menor que rangoFin iremos incrementando hasta que lleguemos a ese número, el incremento/decremento de la luz debe ser gradual
        if (rangoInicio < rangoFin)
        {
            rangoInicio++;
		}
        // si rangoInicio es mayor que rangoFin iremos decrementando hasta que lleguemos a ese número, el incremento/decremento de la luz debe ser gradual
        else if (rangoInicio > rangoFin)
		{
            rangoInicio--;
		}

        // si intensidadInicio es menor que intensidadFin iremos incrementando hasta que lleguemos a ese número, el incremento/decremento de la luz debe ser gradual
        if (intensidadInicio < intensidadFin)
        {
            intensidadInicio++;
        }
        // si intensidadInicio es mayor que intensidadFin iremos decrementando hasta que lleguemos a ese número, el incremento/decremento de la luz debe ser gradual
        else if (intensidadInicio > intensidadFin)
        {
            intensidadInicio--;
        }

        // modificamos el rango
        _luz.range = rangoInicio;
        // modificamos la intensidad
        _luz.intensity = intensidadInicio;

        // si rangoInicio es igual a rangoFin generamos aleatoriamente el siguiente rangoFin
        if (rangoInicio == rangoFin)
		{
            rangoFin = Random.Range(rangoMinimo, rangoMaximo);
        }

        // si intensidadInicio es igual a intensidadFin generamos aleatoriamente el siguiente intensidadFin
        if (intensidadInicio == intensidadFin)
        {
            intensidadFin = Random.Range(intensidadMinima, intensidadMaxima);
        }
    }
}