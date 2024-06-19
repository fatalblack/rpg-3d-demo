using UnityEngine;

public class JugadorMover : MonoBehaviour
{
    // variables p�blicas
    public float velocidad = 4f;
    public float fuerzaSalto = 300f;

    // variables privadas
    Animator _animador;
    Rigidbody _cuerpo;
    float horizontal;
    float vertical;
    float ultimoGradoRotacion = 0;

    // Start is called before the first frame update
    void Start()
    {
        _animador = GetComponent<Animator>();
        _cuerpo = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // si el juego no inicio, no realizamos acci�n alguna
        if (!ValoresGlobales.JuegoIniciado)
        {
            return;
        }

        // si el jugador est� muerto, en batalla, minando o talando no puede hacer nada m�s
        if (
            !_animador.GetBool(AnimadorParametros.Vivo) ||
            _animador.GetBool(AnimadorParametros.EnBatalla) ||
            _animador.GetBool(AnimadorParametros.Minando) ||
            _animador.GetBool(AnimadorParametros.Talando))
		{
            return;
		}

        // actualizamos los valores de horizontal y vertical en base al input de cada eje
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // ejecutamos las acciones que implican el caminar
        AccionesCaminar();

        // ejecutamos las acciones que implican el saltar
        AccionesSaltar();

        // ejecutamos las acciones que implican el rotar
        AccionesRotar();
    }

	private void OnCollisionEnter(Collision colision)
	{
		// si colisionamos contra un objeto que tenga tag piso significa que ya no estamos saltando
		if (colision.gameObject.CompareTag(Tags.Piso))
		{
            _animador.SetBool(AnimadorParametros.Saltando, false);
        }
    }

	private void AccionesSaltar()
	{
        // si presiona barra espaciadora saltamos
		if (Input.GetKeyDown(KeyCode.Space))
		{
            // avisamos al animador que estamos saltando
            _animador.SetBool(AnimadorParametros.Saltando, true);

            // saltamos
            _cuerpo.AddForce(0, fuerzaSalto, 0, ForceMode.Impulse);
        }
    }

    private void AccionesCaminar()
    {
        // avisamos al animador si estamos caminando o no
        _animador.SetBool(AnimadorParametros.Caminando, horizontal != 0 || vertical != 0);

        // si estamos movi�ndonos en cualquiere direcci�n procedemos
        if (vertical != 0 || horizontal != 0)
        {
            // caminamos hacia adelante
            transform.Translate(Vector3.forward * Time.deltaTime * velocidad);
        }
    }

    private void AccionesRotar()
    {
        float? gradoEquivalente = null;

        // rotamos el modelo del jugador para el lado que corresponda
        // arriba
        if (vertical > 0 && horizontal == 0 && ultimoGradoRotacion != GradosMovimiento.GradosArriba)
        {
            gradoEquivalente = GradosMovimiento.GradosArriba;
        }
        // arriba derecha
        if (vertical > 0 && horizontal > 0 && ultimoGradoRotacion != GradosMovimiento.GradosArribaDerecha)
        {
            gradoEquivalente = GradosMovimiento.GradosArribaDerecha;
        }
        // arriba izquierda
        if (vertical > 0 && horizontal < 0 && ultimoGradoRotacion != GradosMovimiento.GradosArribaIzquierda)
        {
            gradoEquivalente = GradosMovimiento.GradosArribaIzquierda;
        }
        // abajo
        if (vertical < 0 && horizontal == 0 && ultimoGradoRotacion != GradosMovimiento.GradosAbajo)
        {
            gradoEquivalente = GradosMovimiento.GradosAbajo;
        }
        // abajo derecha
        if (vertical < 0 && horizontal > 0 && ultimoGradoRotacion != GradosMovimiento.GradosAbajoDerecha)
        {
            gradoEquivalente = GradosMovimiento.GradosAbajoDerecha;
        }
        // abajo izquierda
        if (vertical < 0 && horizontal < 0 && ultimoGradoRotacion != GradosMovimiento.GradosAbajoIzquierda)
        {
            gradoEquivalente = GradosMovimiento.GradosAbajoIzquierda;
        }
        // derecha
        if (vertical == 0 && horizontal > 0 && ultimoGradoRotacion != GradosMovimiento.GradosDerecha)
        {
            gradoEquivalente = GradosMovimiento.GradosDerecha;
        }
        // izquierda
        if (vertical == 0 && horizontal < 0 && ultimoGradoRotacion != GradosMovimiento.GradosIzquierda)
        {
            gradoEquivalente = GradosMovimiento.GradosIzquierda;
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

            // establecemos el �ltimo grado de rotaci�n del jugador, esto servir� para posicionar luego el enemigo actual y que nos mire
            GameManager.Instance.EstablecerUltimoGradoRotacionJugador(ultimoGradoRotacion);
        }
    }
}