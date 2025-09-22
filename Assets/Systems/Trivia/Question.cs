using UnityEngine;

[System.Serializable]
public class Question
{
    public string questionText; // Texto de la pregunta (edita en Inspector)
    public string[] answers = new string[4]; // Textos de las 4 respuestas (edita en Inspector)
    public int correctIndex; // Índice de la respuesta correcta (0-3, edita en Inspector)
}