using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieRollManager : MonoBehaviour
{
    public DiceThrower _diceThrower;
    public Text _text;

    public void RollDice()
    {
        void ShowResult(int result)
        {
            _text.text = result.ToString();
        }

        _text.text = "";
        _diceThrower.RollDie(ShowResult);
    }
}
