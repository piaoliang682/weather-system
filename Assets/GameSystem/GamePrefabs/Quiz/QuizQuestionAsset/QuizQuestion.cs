using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class QuizQuestion : ScriptableObject
{
    public string questionText;
    public Sprite questionSprite;
    public string[] answers;      // must have exactly 4 entries
    public int correctAnswerIndex; // 0 to 3
}
