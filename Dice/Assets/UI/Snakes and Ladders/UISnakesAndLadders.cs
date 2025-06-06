using SnakesAndLadders;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UISnakesAndLadders : MonoBehaviour
{
    [SerializeField] SnakesAndLaddersManager _gameManager;

    VisualElement _ui;

    Button _rollSixDieButton;

    private void Awake()
    {
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _rollSixDieButton = _ui.Q<Button>("RollSixDieButton");

    }

    private void OnEnable()
    {
        _rollSixDieButton.RegisterCallback<ClickEvent>(_ => _gameManager.RollDice());
    }

    private void OnDisable()
    {
        _rollSixDieButton.UnregisterCallback<ClickEvent>(_ => _gameManager.RollDice());
    }
}
