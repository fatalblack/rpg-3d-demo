using UnityEngine;

public class OcultarParedes : MonoBehaviour
{
    // variables p�blicas
    public float limiteZ = -8.6f;

    // variables privadas
    GameObject[] _paredes;

    // Start is called before the first frame update
    void Start()
    {
        // buscamos todas las paredes con el tag ParedAtras para manipularlas despu�s
        _paredes = GameObject.FindGameObjectsWithTag(Tags.ParedAtras);
    }

    // Update is called once per frame
    void Update()
    {
        // si pasamos el l�mite determinado ocultamos las paredes
		if (transform.position.z < limiteZ)
		{
            // recorremos cada pared encontrada
			foreach (GameObject pared in _paredes)
			{
                // ocultamos el render de la pared
                pared.GetComponent<MeshRenderer>().forceRenderingOff = true;
			}
		}
		else
		{
            // recorremos cada pared encontrada
            foreach (GameObject pared in _paredes)
            {
                // activamos el render de la pared
                pared.GetComponent<MeshRenderer>().forceRenderingOff = false;
            }
        }
    }
}