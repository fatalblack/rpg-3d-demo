using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// parametros desde públicos
	public int IdPersonaje = 1;
	public int InventarioMaximo = 35;
	public int ApilableMaximo = 9999;
	public ReproductorPanel reproductorPanel;

	// variables privadas
	private string nombrePersonaje;

	// instancia de GameManager para singleton
	public static GameManager Instance { get; private set; }

	// parametros relevantes para recoger data en el juego
	public Personaje Jugador { get { return _jugador; } }

	private Personaje _jugador;

	public Animator AnimadorJugador { get { return _animadorJugador; } }

	private Animator _animadorJugador;

	public Enemigo EnemigoActual { get { return _enemigoActual; } }

	private Enemigo _enemigoActual;

	private Enemigo _enemigoMuerto;

	public Animator AnimadorEnemigoActual { get { return _animadorEnemigoActual; } }

	private Animator _animadorEnemigoActual;

	private Animator _animadorEnemigoMuerto;

	public ResultadoBatalla ResultadoUltimaBatalla { get { return _resultadoUltimaBatalla; } }

	private ResultadoBatalla _resultadoUltimaBatalla;

	public Recolectable RecolectableActual { get { return _recolectableActual; } }

	private Recolectable _recolectableActual;

	public Animator AnimadorRecolectableActual { get { return _animadorRecolectableActual; } }

	private Animator _animadorRecolectableActual;

	public List<Objeto> Tienda { get { return _tienda; } }

	private List<Objeto> _tienda;

	private bool InventarioSincronizado { get; set; }

	public int CantidadMaximaEspaciosInventario { get { return InventarioMaximo; } }

	public int CantidadMaximaObjetosApilables { get { return ApilableMaximo; } }

	public bool PuedeVender { get { return _puedeVender; } }

	private bool _puedeVender;

	private Vector3 _jugadorPosicionInicioJuego;

	private Vector3 _jugadorPosicionOriginal;

	private float _jugadorPosicionHorizontalOrigen;

	private Vector3? _enemigoPosicionOriginal;

	private GameObject _escenarioBatalla;

	private float _ultimoGradoRotacionJugador = 0;

	// inicializamos el singleton en caso de no estarlo
	private void Awake()
	{
		if (Instance == null)
		{
			// creamos el personaje para el jugador
			_jugador = CreadorPersonajes.Crear(IdPersonaje, nombrePersonaje);
			
			// establecemos la posición donde inició el jugador, para respawn en caso de morir
			_jugadorPosicionInicioJuego = GameObject.FindGameObjectWithTag(Tags.Jugador).transform.position;

			// creamos el enemigo en nulo inicialmente
			_enemigoActual = null;

			// renovamos stock de la tienda
			_tienda = TiendaAcciones.ObtenerStockTienda();

			// obtenemos el GameObject del escenario de batalla
			EstablecerEscenarioBatalla();

			// dejamos la instancia como el estado actual del GameManager
			Instance = this;

			// le ponemos por defecto 2 pociones menores de vida
			PersonajeInventarioAcciones.AgregarObjetoInventario(CreadorObjetos.CrearPocionVidaMenor(), 2);
		}
	}

	public void EstablecerAnimadorJugador(Animator animator)
	{
		_animadorJugador = animator;
		_jugadorPosicionOriginal = animator.transform.position;
	}

	public void EstablecerEnemigoActual(Enemigo enemigo)
	{
		_enemigoActual = enemigo;
	}

	public void EstablecerAnimadorEnemigoActual(Animator animator)
	{
		_animadorEnemigoActual = animator;
		_enemigoPosicionOriginal = new Vector3(animator.transform.position.x, animator.transform.position.y);
	}

	public void EstablecerInicioBatalla()
	{
		// actualizamos la posición actual del enemigo
		EstablecerEnemigoPosicionOriginal(_animadorEnemigoActual.transform.position);

		// mostramos el escenario de batalla
		MostrarEscenarioBatalla();

		// ponemos el estado de "EnBatalla del jugador" y el enemigo en true
		_animadorJugador.SetBool(AnimadorParametros.EnBatalla, true);
		_animadorJugador.SetBool(AnimadorParametros.Caminando, false);
		_animadorEnemigoActual.SetBool(AnimadorParametros.EnBatalla, true);
	}

	public void EstablecerResultadoUltimaBatalla(ResultadoBatalla resultadoBatalla)
	{
		_resultadoUltimaBatalla = resultadoBatalla;
	}

	public void EstablecerFinBatalla()
	{
		// ocultamos el escenario de batalla
		OcultarEscenarioBatalla();

		// avisamos que finalizó la batalla
		_animadorJugador.SetBool(AnimadorParametros.EnBatalla, false);

		// si el enemigo murió destruímos el objeto
		if (_animadorEnemigoActual == null)
		{
			// destruimos el objeto del enemigo
			StartCoroutine(DestruirGameObjectEnemigo());
		}
		else
		{
			_animadorEnemigoActual.SetBool(AnimadorParametros.EnBatalla, false);
		}
	}

	public void EstablecerRecolectableActual(Recolectable recolectable)
	{
		_recolectableActual = recolectable;
	}

	public void EstablecerAnimadorRecolectableActual(Animator animator)
	{
		_animadorRecolectableActual = animator;
	}

	public void MatarYRevivirJugador()
	{
		// mostramos el canvas de perdiste
		AccionPerderdor.Perdiste();

		// seteamos los datos del jugador indicando que murió
		_animadorJugador.SetBool(AnimadorParametros.EnBatalla, false);
		_animadorJugador.SetBool(AnimadorParametros.Vivo, false);

		// revivimos al jugador
		StartCoroutine(RevivirJugador());
	}

	private System.Collections.IEnumerator RevivirJugador()
	{
		// si el jugador está muerto procedemos
		if (!_animadorJugador.GetBool(AnimadorParametros.Vivo))
		{
			// agregamos un delay antes de revivir cosa que se vea el cadaver xD
			yield return new WaitForSeconds(2);

			// restauramos la vida del jugador
			PersonajeAcciones.CurarVida(_jugador.Estadistica.VidaMaximaCalculada);
			_animadorJugador.SetBool(AnimadorParametros.Vivo, true);

			// hacemos respawn del jugador en el punto donde inició el juego
			_animadorJugador.transform.position = _jugadorPosicionInicioJuego;
		}
	}

	public void MatarEnemigo()
	{
		// seteamos el enemigo muerto como historial y para revivirlo de ser necesario
		_animadorEnemigoMuerto = _animadorEnemigoActual;
		_enemigoMuerto = _enemigoActual;

		// al haber muerto el enemigo liberamos toda animación de él que indicaba batalla y vida
		_animadorEnemigoActual.SetBool(AnimadorParametros.EnBatalla, false);
		_animadorEnemigoActual.SetBool(AnimadorParametros.Vivo, false);

		// desasignamos el enemigo del manager
		_animadorEnemigoActual = null;
		_enemigoActual = null;
	}

	public void ActualizarEstadoAtaqueAEnemigo(bool ataqueExitoso)
	{
		// actualizamos estado de ataque en el enemigo
		_animadorEnemigoActual.SetBool(AnimadorParametros.AdvertirAtaqueEnemigo, true);
		_animadorEnemigoActual.SetBool(AnimadorParametros.Golpeado, ataqueExitoso);
		_animadorEnemigoActual.SetBool(AnimadorParametros.Atacando, false);

		// informamos que el jugador no está siendo atacado
		_animadorJugador.SetBool(AnimadorParametros.AdvertirAtaqueEnemigo, false);
		_animadorJugador.SetBool(AnimadorParametros.Golpeado, false);
		_animadorJugador.SetBool(AnimadorParametros.Atacando, true);
	}

	public void ActualizarEstadoAtaqueAJugador(bool ataqueExitoso)
	{
		// actualizamos estado de ataque en el jugador
		_animadorJugador.SetBool(AnimadorParametros.AdvertirAtaqueEnemigo, true);
		_animadorJugador.SetBool(AnimadorParametros.Golpeado, ataqueExitoso);
		_animadorJugador.SetBool(AnimadorParametros.Atacando, false);

		// informamos que el enemigo no está siendo atacado
		_animadorEnemigoActual.SetBool(AnimadorParametros.AdvertirAtaqueEnemigo, false);
		_animadorEnemigoActual.SetBool(AnimadorParametros.Golpeado, false);
		_animadorEnemigoActual.SetBool(AnimadorParametros.Atacando, true);
	}

	public void ReiniciarEstadoAtaqueAJugadorYEnemigo()
	{
		// quitamos el etado de atacante del jugador
		_animadorJugador.SetBool(AnimadorParametros.AdvertirAtaqueEnemigo, false);
		_animadorJugador.SetBool(AnimadorParametros.Golpeado, false);
		_animadorJugador.SetBool(AnimadorParametros.Atacando, false);

		// quitamos el etado de atacante del enemigo
		_animadorEnemigoActual.SetBool(AnimadorParametros.AdvertirAtaqueEnemigo, false);
		_animadorEnemigoActual.SetBool(AnimadorParametros.Golpeado, false);
		_animadorEnemigoActual.SetBool(AnimadorParametros.Atacando, false);
	}

	public void InventarioModificado()
	{
		InventarioSincronizado = false;
	}

	public bool SeDebeSincronizarInventario()
	{
		return !InventarioSincronizado;
	}

	public void SeSincronizoInventario()
	{
		InventarioSincronizado = true;
	}

	public void EstablecerPuedeVender(bool puedeVender)
	{
		_puedeVender = puedeVender;
	}

	public void EstablecerJugadorPosicionOriginal(Vector3 posicionActual)
	{
		_jugadorPosicionOriginal = posicionActual;
	}

	public void EstablecerJugadorPosicionHorizontalOrigen(float posicionOrigen)
	{
		_jugadorPosicionHorizontalOrigen = posicionOrigen;
	}

	public void EstablecerEnemigoPosicionOriginal(Vector3 posicionActual)
	{
		_enemigoPosicionOriginal = posicionActual;
	}

	public void EstablecerUltimoGradoRotacionJugador(float ultimoGradoRotacionJugador)
	{
		// establecemos el último grado de rotación del jugador, esto servirá para posicionar luego el enemigo actual y que nos mire
		_ultimoGradoRotacionJugador = ultimoGradoRotacionJugador;
	}

	// giramos la vista del enemigo para que nos mire en batalla
	public void GirarVistaEnemigoActual()
	{
		Transform enemigoTransform = _animadorEnemigoActual.transform;
		// por cada ángulo que tenga el jugador, habrá una equivalencia para el enemigo
		float gradoRotacionEquivalente = GradosMovimiento.ObtenerGradoOpuesto(_ultimoGradoRotacionJugador);

		// reiniciamos el ángulo de rotación del enemigo así solo sumamos el ángulo equivalente
		enemigoTransform.rotation = Quaternion.identity;

		// establecemos el grado de rotación correspondiente para el enemigo
		enemigoTransform.Rotate(Vector3.up, gradoRotacionEquivalente);
	}

	private void EstablecerEscenarioBatalla()
	{
		// obtenemos el GameObject del escenario de batalla
		_escenarioBatalla = GameObject.FindGameObjectWithTag(Tags.EscenarioBatalla);

		// ocultamos por defecto el escenario
		OcultarEscenarioBatalla();
	}

	private void MostrarEscenarioBatalla()
	{
		// mostramos el escenario de batalla
		_escenarioBatalla.SetActive(true);
	}

	private void OcultarEscenarioBatalla()
	{
		// ocultamos el escenario de batalla
		_escenarioBatalla.SetActive(false);
	}

	private System.Collections.IEnumerator DestruirGameObjectEnemigo()
	{
		// destruimos el objeto del enemigo a los 2 segundos para dar tiempo a la animación de morir
		yield return new WaitForSeconds(2);
		GameObject.Destroy(_animadorEnemigoMuerto.gameObject);
	}
}