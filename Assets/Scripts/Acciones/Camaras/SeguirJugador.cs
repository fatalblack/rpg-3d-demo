using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    // variables públicas
    public GameObject jugador;
    public float distanciaX = 0f;
    public float distanciaY = 3f;
    public float distanciaZ = -5f;

    // variables privadas
    private Vector3 distancia;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
        distancia = new Vector3(distanciaX, distanciaY, distanciaZ);
    }

	void LateUpdate()
    {
        // movemos la cámara para que siga al jugador
        transform.position = jugador.transform.position + distancia;
    }
}