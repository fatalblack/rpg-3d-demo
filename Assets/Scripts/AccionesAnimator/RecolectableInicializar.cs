using UnityEngine;

public class RecolectableInicializar : MonoBehaviour
{
	//Detect collisions between the GameObjects with Colliders attached
	private void OnTriggerEnter()
	{
		// al haber colisi�n debemos activar la alerta
		// TODO: implementar alguna se�al para saber que estamos colisionando con el recolectable
		// _alertaAnimador.SetBool(AnimadorParametros.EstaColisionado, true);
	}

	private void OnTriggerExit()
	{
		// al no haber colisi�n debemos desactivar la alerta
		// TODO: implementar alguna se�al para saber que estamos colisionando con el recolectable
		// _alertaAnimador.SetBool(AnimadorParametros.EstaColisionado, false);

		// vaciamos el recolectable actual del GameManager
		GameManager.Instance.EstablecerAnimadorRecolectableActual(null);
		GameManager.Instance.EstablecerRecolectableActual(null);
	}
}