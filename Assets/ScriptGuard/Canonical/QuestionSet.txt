using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestionSet", menuName = "Quiz/Question Set")]
public class QuestionSet : ScriptableObject
{
    [Tooltip("List of questions in this set")]
    public QuizQuestion[] questions;

    /// <summary>
    /// Get a random question from the set.
    /// </summary>
    public QuizQuestion GetRandomQuestion()
    {
        if (questions == null || questions.Length == 0)
            return null;
        return questions[Random.Range(0, questions.Length)];
    }

    /// <summary>
    /// Shuffle questions and return a new array.
    /// </summary>
    public QuizQuestion[] GetShuffledQuestions()
    {
        var arr = (QuizQuestion[])questions.Clone();
        for (int i = 0; i < arr.Length; i++)
        {
            int j = Random.Range(i, arr.Length);
            var tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
        return arr;
    }

    public QuizQuestion[] GetQuestionPack()
    {
        var arr = (QuizQuestion[])questions.Clone();
        return arr;
    }
}
