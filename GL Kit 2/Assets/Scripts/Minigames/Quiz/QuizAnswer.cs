using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quiz Answer", menuName = "Quiz Answer")]
public class QuizAnswer : ScriptableObject
{
    public Sprite sprite;
    public string antwoord;
    public List<string> randomAnswers;
}
