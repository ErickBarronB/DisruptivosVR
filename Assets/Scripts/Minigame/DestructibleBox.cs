using UnityEngine;

namespace Minigame
{
    /// <summary>
    /// Componente que se encarga de destruir la caja cuando entra en contacto con las manos del jugador (Meta XR).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DestructibleBox : MonoBehaviour
    {
        [Header("Configuración del Jugador")]
        [Tooltip("El tag que deben tener los colliders de la mano para destruir esta caja.")]
        public string handTag = "Hand";

        [Header("Efectos Visuales")]
        [Tooltip("Prefab de partículas a instanciar cuando la caja es destruida (Opcional).")]
        public GameObject destructionParticlesPrefab;

        private void OnTriggerEnter(Collider other)
        {
            // Comprobamos si el objeto que entró en el trigger tiene el tag asignado para las manos.
            if (other.CompareTag(handTag))
            {
                DestroyBox();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // En caso de que se use un sistema de colisiones estrictamente físicas (no triggers).
            if (collision.gameObject.CompareTag(handTag))
            {
                DestroyBox();
            }
        }

        private void DestroyBox()
        {
            // Si hay un sistema de partículas asignado, instanciarlo.
            if (destructionParticlesPrefab != null)
            {
                Instantiate(destructionParticlesPrefab, transform.position, transform.rotation);
            }

            // Destruir este objeto.
            Destroy(gameObject);
        }
    }
}
