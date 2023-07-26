using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private VisualElement _visualElement;
    private Label _scoreLabel;
    private int _score = 0;

    private void OnEnable()
    {
        _visualElement = GetComponent<UIDocument>().rootVisualElement;
        _scoreLabel = _visualElement.Q<Label>("Score");
        UpdateScoreText();
    }
    
    public void IncreaseScore(int points)
    {
        _score += points;
        UpdateScoreText();
    }
    
    private void UpdateScoreText()
    {
        _scoreLabel.text = "Score: " + _score.ToString();
    }
}
