using UnityEngine;

public class RecolectableInicializar : MonoBehaviour
{
	//Detect collisions between the GameObjects with Colliders attached
	private void OnTriggerEnter()
	{
		// al haber colisión debemos activar la alerta
		// TODO: implementar alguna señal para saber que estamos colisionando con el recolectable
		// _alertaAnimador.SetBool(AnimadorParametros.EstaColisionado, true);
	}

	private void OnTriggerExit()
	{
		// al no haber colisión debemos desactivar la alerta
		// TODO: implementar alguna señal para saber que estamos colisionando con el recolectable
		// _alertaAnimador.SetBool(AnimadorParametros.EstaColisionado, false);

		// vaciamos el recolectable actual del GameManager
		GameManager.Instance.EstablecerAnimadorRecolectableActual(null);
		GameManager.Instance.EstablecerRecolectableActual(null);
	}
}