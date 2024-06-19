using System.Collections.Generic;
using UnityEngine;

public class EnemigoMover : MonoBehaviour
{
	// variables p�blicas
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

		// repetimos la acci�n de rotar (cambiar direcci�n) cada tantos segundos como frecuencia se haya establecido
		// en el GAMEMANAGER tambi�n se rota GirarVistaEnemigoActual
		InvokeRepeating(nameof(AccionesRotar), 1f, frecuencia);
	}

	// Update is called once per frame
	void Update()
	{
		// repetimos la acci�n de movernos
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

		// Simularemos la direcci�n de movimiento del enemigo
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

		// simulamos la "tecla" que se presionar�a en el enemigo
		int indiceMovimiento = Random.Range(0, movimientos.Count);

		if (volvimosAlOrigen)
		{
			// establecemos el valor de gradoEquivalente en base a la aleatoriedad del �ndice
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

		// si el gradoEquivalente cambi� (distinto de nulo) hay que actualizar la posici�n
		if (gradoEquivalente.HasValue)
		{
			// reiniciamos el �ngulo de rotaci�n del enemigo as� solo sumamos el �ngulo equivalente
			transform.rotation = Quaternion.identity;

			// rotamos al �ngulo equivalente
			transform.Rotate(Vector3.up, gradoEquivalente.Value);

			// actualizamos el �ltimo �ngulo donde nos movimos
			ultimoGradoRotacion = gradoEquivalente.Value;
		}
	}

	private bool PuedeCaminar()
	{
		// si el enemigo est� muerto, o en batalla no puede hacer nada m�s
		if (
			!_animador.GetBool(AnimadorParametros.Vivo) ||
			_animador.GetBool(AnimadorParametros.EnBatalla))
		{
			// avisamos al animador que no est� caminando el enemigo
			_animador.SetBool(AnimadorParametros.Caminando, false);

			return false;
		}

		return true;
	}
}