using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIDiceRoll : MonoBehaviour
{
    [SerializeField] DieRollManager _dieRollManager;

    VisualElement _ui;

    Button _rollSixDieButton;
    Button _flipCoinButton;
    Button _clearButton;
    Label _resultLabel;

    private void Awake()
    {
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _rollSixDieButton = _ui.Q<Button>("RollSixDieButton");
        _flipCoinButton = _ui.Q<Button>("FlipCoinButton");
        _clearButton = _ui.Q<Button>("ClearButton");
        _resultLabel = _ui.Q<Label>("ResultLabel");
    }

    private void OnEnable()
    {
        _rollSixDieButton.RegisterCallback<ClickEvent>(_ => _dieRollManager.RollDice());
        _flipCoinButton.RegisterCallback<ClickEvent>(_ => _dieRollManager.FlipCoint());
        _clearButton.RegisterCallback<ClickEvent>(_ => _dieRollManager.ClearAssets());

        _dieRollManager.OnResultFinished += ShowResult;
    }

    private void OnDisable()
    {
        _rollSixDieButton.UnregisterCallback<ClickEvent>(_ => _dieRollManager.RollDice());
        _flipCoinButton.UnregisterCallback<ClickEvent>(_ => _dieRollManager.FlipCoint());
        _clearButton.UnregisterCallback<ClickEvent>(_ => _dieRollManager.ClearAssets());

        _dieRollManager.OnResultFinished -= ShowResult;
    }

    private void ShowResult(string text)
    {
        _resultLabel.text = text;
    }
}
