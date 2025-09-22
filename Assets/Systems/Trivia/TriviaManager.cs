using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TriviaManager : MonoBehaviour
{
    public Question[] questions; // Array de preguntas (agrega/quita/edit en Inspector)
    public TextMeshProUGUI questionText; // Asigna el TMP de la pregunta
    public Button[] answerButtons; // Asigna los 4 botones de respuestas
    public TextMeshProUGUI[] answerTexts; // Asigna los TMP hijos de los botones

    private int currentQuestionIndex = 0;
    private bool hasAnswered = false;

    void Start()
    {
        if (questions.Length == 0) return; // No hay preguntas
        LoadQuestion(currentQuestionIndex);
    }

    private void LoadQuestion(int index)
    {
        Question q = questions[index];
        questionText.text = q.questionText;

        for (int i = 0; i < 4; i++)
        {
            answerTexts[i].text = q.answers[i];
            int answerIndex = i; // Captura para lambda
            answerButtons[i].onClick.RemoveAllListeners(); // Limpia listeners previos
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answerIndex, q.correctIndex));
            answerButtons[i].interactable = true; // Habilita botones
            answerButtons[i].image.color = Color.white; // Reset color
        }
    }

    private void OnAnswerSelected(int selectedIndex, int correctIndex)
    {
        if (hasAnswered) return;
        hasAnswered = true;

        bool isCorrect = selectedIndex == correctIndex;
        Color feedbackColor = isCorrect ? Color.green : Color.red;

        // Cambia color del botón seleccionado por 3 segundos
        answerButtons[selectedIndex].image.color = feedbackColor;
        StartCoroutine(ResetButtonColorAfterDelay(selectedIndex, 3f));

        // Deshabilita todos los botones para evitar multiclics
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }

        // Espera 5 segundos y pasa a siguiente o regresa
        StartCoroutine(NextQuestionAfterDelay(5f));
    }

    private IEnumerator ResetButtonColorAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        answerButtons[index].image.color = Color.white;
    }

    private IEnumerator NextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasAnswered = false;

        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Length)
        {
            LoadQuestion(currentQuestionIndex);
        }
        else
        {
            // No más preguntas: Regresa a "Game"
            SceneManager.LoadScene("Game");
        }
    }
}