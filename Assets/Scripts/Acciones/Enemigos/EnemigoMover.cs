using System.Collections.Generic;
using UnityEngine;

public class EnemigoMover : MonoBehaviour
{
	// variables públicas
	public float velocidad = 1;
	public float rango = 1;
	public float frecuencia = 3;

	// variables privadas
	private Animator _animador;
	float ultimoGradoRotacion = 0;
	bool volvimosAlOrigen = false;

	// Use this for initialization
	void Start()
	{
		_animador = gameObject.GetComponent<Animator>();

		// repetimos la acción de rotar (cambiar dirección) cada tantos segundos como frecuencia se haya establecido
		// en el GAMEMANAGER también se rota GirarVistaEnemigoActual
		InvokeRepeating(nameof(AccionesRotar), 1f, frecuencia);
	}

	// Update is called once per frame
	void Update()
	{
		// repetimos la acción de movernos
		AccionesCaminar();
	}

	private void AccionesCaminar()
	{
		// validamos si puede caminar
		if (!PuedeCaminar())
		{
			return;
		}

		// avisamos al animador si estamos caminando o no
		_animador.SetBool(AnimadorParametros.Caminando, true);

		// caminamos hacia adelante
		transform.Translate(Vector3.forward * Time.deltaTime * velocidad * rango);
	}

	private void AccionesRotar()
	{
		// validamos si puede caminar
		if (!PuedeCaminar())
		{
			return;
		}

		float? gradoEquivalente;

		// Simularemos la dirección de movimiento del enemigo
		// para ello armaremos una lista con las posiciones disponbles (GradosMovimiento)
		List<float> movimientos = new List<float>
		{
			GradosMovimiento.GradosArriba,
			GradosMovimiento.GradosArribaDerecha,
			GradosMovimiento.GradosDerecha,
			GradosMovimiento.GradosAbajoDerecha,
			GradosMovimiento.GradosAbajo,
			GradosMovimiento.GradosAbajoIzquierda,
			GradosMovimiento.GradosIzquierda,
			GradosMovimiento.GradosArribaIzquierda
		};

		// simulamos la "tecla" que se presionaría en el enemigo
		int indiceMovimiento = Random.Range(0, movimientos.Count);

		if (volvimosAlOrigen)
		{
			// establecemos el valor de gradoEquivalente en base a la aleatoriedad del índice
			gradoEquivalente = movimientos[indiceMovimiento];

			// marcamos que nos fuimos del punto de origen
			volvimosAlOrigen = false;
		}
		else
		{
			// volvemos al punto de origen haciendo el recorrido contrario al anterior
			gradoEquivalente = GradosMovimiento.ObtenerGradoOpuesto(ultimoGradoRotacion);

			// marcamos que volvimos
			volvimosAlOrigen = true;
		}

		// si el gradoEquivalente cambió (distinto de nulo) hay que actualizar la posición
		if (gradoEquivalente.HasValue)
		{
			// reiniciamos el ángulo de rotación del enemigo así solo sumamos el ángulo equivalente
			transform.rotation = Quaternion.identity;

			// rotamos al ángulo equivalente
			transform.Rotate(Vector3.up, gradoEquivalente.Value);

			// actualizamos el último ángulo donde nos movimos
			ultimoGradoRotacion = gradoEquivalente.Value;
		}
	}

	private bool PuedeCaminar()
	{
		// si el enemigo está muerto, o en batalla no puede hacer nada más
		if (
			!_animador.GetBool(AnimadorParametros.Vivo) ||
			_animador.GetBool(AnimadorParametros.EnBatalla))
		{
			// avisamos al animador que no está caminando el enemigo
			_animador.SetBool(AnimadorParametros.Caminando, false);

			return false;
		}

		return true;
	}
}