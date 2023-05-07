using System.Collections;
using UnityEngine;

namespace SnakesAndLadders
{
    [RequireComponent(typeof(Board))]
    public class GameManager : MonoBehaviour
    {
        public DiceThrower _diceThrower;
        public Player[] _players;
        public int _rows;
        public int _collums;
        [Min(0.0001f)] public float _hopTime = 0.4f;
        [Min(0.0001f)] public float _pauseHopTime = 0.1f;

        public Board _board;

        private int _playerCounter = 0;

        private void Awake()
        {
            _board = GetComponent<Board>();
            _board._rows = _rows;
            _board._collums = _collums;

            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].placeOnBoard = 1;
            }
        }

        public void RollDice()
        {
            void RunMovePiece(int dieResult)
            {
                StartCoroutine(MovePiece(dieResult));
            }

            _diceThrower.RollDie(RunMovePiece);
            GetNextPlayer(1);
        }

        private IEnumerator MovePiece(int dieResult)
        {
            float stepTime = 1f / _hopTime;
            int placeOnBoard = _players[_playerCounter].placeOnBoard;
            Player currentPlayer = _players[_playerCounter];
            Transform playerTr = currentPlayer.transform;
            int newPlace = placeOnBoard + dieResult;
            newPlace = Mathf.Min(newPlace, _rows * _collums);

            for (int i = placeOnBoard; i < newPlace; i++)
            {
                float elapsed = 0;
                Vector3 previousPosition = currentPlayer.transform.position;
                int x = i % _rows;
                int y = i / _collums;
                Vector3 nextPosition = _board.TilePosition(x, y);

                while (elapsed < _hopTime)
                {
                    playerTr.position = Vector3.Lerp(previousPosition, nextPosition, elapsed / _hopTime);
                    elapsed += Time.deltaTime * stepTime;
                    yield return null;
                }

                playerTr.position = nextPosition;
                yield return new WaitForSeconds(_pauseHopTime);
            }

            currentPlayer.placeOnBoard = newPlace;
            yield break;
        }

        public Player GetNextPlayer(int turns = 1)
        {
            _playerCounter = (_playerCounter + turns) % _players.Length;
            return _players[_playerCounter];
        }
    }
}
