using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptCable
{
    public class CableManager : MonoBehaviour
    {
        public static CableManager Instance;
        public int totalCables;
        public GameObject firstTimeline, nextTimeline;
        public int cablesConnected = 0;
        public bool isPuzzleCompleted = false; // Booleano para indicar que la prueba se pasó

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            // Activa el firstTimeline al cargar la escena
            if (firstTimeline != null) firstTimeline.SetActive(true);
        }

        public void CableConnected()
        {
            cablesConnected++;
            Debug.Log("Cables conectados: " + cablesConnected);

            if (cablesConnected >= totalCables)
            {
                Win();
            }
        }

        private void Win()
        {
            // Desactiva el firstTimeline
            if (firstTimeline != null) firstTimeline.SetActive(false);

            // Activa el estado de victoria y programa el retorno
            if (nextTimeline != null)
            {
                nextTimeline.SetActive(true);
                isPuzzleCompleted = true; // Marca la prueba como completada
                StartCoroutine(WaitAndReturnToGame(5f)); // Espera 5 segundos y regresa
            }
        }

        private IEnumerator WaitAndReturnToGame(float delay)
        {
            yield return new WaitForSeconds(delay); // Espera 5 segundos
            SceneManager.LoadScene("Game"); // Regresa a la escena "Game"
        }
    }
}