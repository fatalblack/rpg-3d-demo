using System.Linq;
using UnityEngine;

public class SpawnerEnemigo : MonoBehaviour
{
    // variables públicas
    public GameObject enemigo;
    public int spawnearDespuesDeXSegundos;

    // variables privadas
    Transform? _enemigoGenerado = null;
    bool spawneando = false;

    // Update is called once per frame
    void Update()
    {
        // si no hay un enemigo y no estamos en proceso de spawnear uno, procedemos
        if (_enemigoGenerado == null && !spawneando)
        {
            // establecemos en el proceso que estamos spawneando
            spawneando = true;

            // instanciamos el enemigo
            StartCoroutine(InstanciarEnemigo());
        }
    }

    private System.Collections.IEnumerator InstanciarEnemigo()
    {
        // ponemos un delay antes de realizar la instancia de X segundos (spawnearDespuesDeXSegundos)
        yield return new WaitForSeconds(spawnearDespuesDeXSegundos);

        // instanciamos el enemigo dentro del padre (el objeto que tenga este script)
        GameObject.Instantiate(enemigo, transform);

        // posicionamos el enemigo en la misma posición que el spawner
        _enemigoGenerado = ObtenerTransformEnemigoGenerado();
        _enemigoGenerado.position = transform.position;

        // avisamos que ya no estamos spawneando
        spawneando = false;
    }

    private Transform? ObtenerTransformEnemigoGenerado()
    {
        // buscamos y retornamos el hijo encontrado, en caso de no haber uno retornamos nulo
        return gameObject.GetComponentsInChildren<Transform>()
            .FirstOrDefault(componente => componente.CompareTag(Tags.Enemigo));
    }
}