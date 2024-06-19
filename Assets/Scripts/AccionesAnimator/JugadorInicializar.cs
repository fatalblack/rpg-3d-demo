using UnityEngine;

public class JugadorInicializar : MonoBehaviour
{
	// variables privadas
	Animator _animador;

	// Start is called before the first frame update
	void Start()
	{
		_animador = GetComponent<Animator>();

		// establecemos el animator en el GameManager
		GameManager.Instance.EstablecerAnimadorJugador(_animador);

		// establecemos si est� vivo en el animador en base al personaje del GameManager
		_animador.SetBool(AnimadorParametros.Vivo, true);
	}

	//Detect collisions between the GameObjects with Colliders attached
	void OnCollisionEnter(Collision colision)
	{
		EvaluarColisionBatalla(colision);
	}

	private void OnTriggerEnter(Collider colision)
	{
		EvaluarColisionRecolectable(colision);
	}

	private void EvaluarColisionBatalla(Collision colision)
	{
		// verificamos si es colisi�n de batalla
		bool esColisionBatalla = JugadorColisionBatalla.DetectarColisionBatalla(colision);

		// si es colisi�n procedemos con los pasos para batallar
		if (esColisionBatalla)
		{
			// iniciamos la batalla
			JugadorColisionBatalla.IniciarBatalla();

			// mostramos la animaci�n de la batalla
			StartCoroutine(JugadorColisionBatalla.AnimacionBatalla());

			// si resultadoBatalla es distinto de null significa que la colisi�n desenvoc� en una batalla
			/*ResultadoBatalla resultadoBatalla = GameManager.Instance.ResultadoUltimaBatalla;
			if (resultadoBatalla != null)
			{
				if (resultadoBatalla.Ganada)
				{
					EventosAcciones.Instancia.AgregarEventoExito($"Ganaste {resultadoBatalla.PremioExperiencia} de exp. y {resultadoBatalla.PremioOro} de oro.");
				}
				else
				{
					EventosAcciones.Instancia.AgregarEventoPeligro("Perdiste la batalla.");
				}
			}*/
		}
	}

	private void EvaluarColisionRecolectable(Collider colision)
	{
		JugadorColisionRecolectable.DetectarColisionRecolectable(colision);
	}
}