using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private Text diceResultText;
    [SerializeField] private Vector3 lauchVelocity;

    private void Awake()
    {
        dice = GameObject.Instantiate(dice);
        dice.gameObject.SetActive(false);
    }

    public void Throw(System.Action<int> callback)
    {
        dice.transform.position = transform.position;
        dice.transform.rotation = Random.rotation;
        dice.gameObject.SetActive(true);
        dice.EvaluateWhenStopped(callback);
    }

    public void ShowResult(int result)
    {
        diceResultText.text = $"{result}";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Throw(ShowResult);
    }
}
