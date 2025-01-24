using Plumbly.Dice;
using UnityEngine;
using UnityEngine.UI;

public class DieRollManager : MonoBehaviour
{
    public DieCaster _diceThrower;
    public CoinThrower _coinThrower;
    public Text _text;

    public void RollDice()
    {
        void ShowResult(int result)
        {
            _text.text = result.ToString();
        }

        _text.text = "";
        _diceThrower.CastNewDie(ShowResult);
    }

    public void FlipCoint()
    {
        void ShowResult(SideOfCoin result)
        {
            _text.text = result.ToString();
        }

        _text.text = "";
        _coinThrower.FlipCoin(ShowResult);
    }
}
