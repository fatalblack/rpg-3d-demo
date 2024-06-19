using UnityEngine;
using UnityEngine.UI;

public class BotonManejarAudioSource : MonoBehaviour
{
    // variables p�blicas
    public AudioSource audioSource;
    public bool muteado;
    public Sprite imagenActivo;
    public Sprite imagenInactivo;

    // variables privadas
    Image _imagenBoton;

	private void Start()
	{
        _imagenBoton = gameObject.GetComponent<Image>();
	}

	public void AlternarMuteo()
	{
        // alternamos el muteo, si estaba muteado ahora ya no y viceversa
        muteado = !muteado;

        // dependiendo si se mute� o no, tomar�mos esa acci�n en el audioSource
        audioSource.mute = muteado;

        // cambiamos la imagen del bot�n de acuerdo a la elecci�n
        _imagenBoton.sprite = muteado ? imagenInactivo : imagenActivo;
	}
}