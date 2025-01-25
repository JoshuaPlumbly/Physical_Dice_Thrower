using Plumbly.Dice;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SnakesAndLadders
{
    [RequireComponent(typeof(Board))]
    public class GameManager : MonoBehaviour
    {
        public DieCaster _dieCaster;
        public Player[] _players;
        public int _rows;
        public int _collumns;
        [Min(0.0001f)] public float _hopTime = 0.4f;
        [Min(0.0001f)] public float _pauseHopTime = 0.1f;

        public Board _board;
        private int _playerCounter = 0;
        public Vector2Int[] _snakes;
        public Vector2Int[] _ladders;

        private Dictionary<int, int> _snakesDict = new Dictionary<int, int>();
        private Dictionary<int, int> _laddersDict = new Dictionary<int, int>();

        private void Awake()
        {
            _board = GetComponent<Board>();
            _board._rows = _rows;
            _board._columns = _collumns;

            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].placeOnBoard = 1;
                _players[i].transform.position = _board.TilePosition(_players[i].placeOnBoard);
            }

            for (int i = 0; i < _snakes.Length; i++)
            {
                _snakesDict.Add(_snakes[i].x, _snakes[i].y);
            }

            for (int i = 0; i < _ladders.Length; i++)
            {
                _laddersDict.Add(_ladders[i].x, _ladders[i].y);
            }
        }

        public void RollDice()
        {
            _dieCaster.CastDie(MovePieceAsync);
            CyclePlayerTurn();
        }

        private async void MovePieceAsync(int dieResult)
        {
            int placeOnBoard = _players[_playerCounter].placeOnBoard;
            Player currentPlayer = _players[_playerCounter];
            Transform playerTr = currentPlayer.transform;
            int placeAfterRoll = placeOnBoard + dieResult;
            placeAfterRoll = Mathf.Min(placeAfterRoll, _rows * _collumns);
            int newPlace = placeAfterRoll;

            for (int i = placeOnBoard; i <= placeAfterRoll; i++)
            {
                Vector3 nextPosition = _board.TilePosition(i);
                await MoveTransformAsync(playerTr, nextPosition, _hopTime);
                await DelayForSecounds(_pauseHopTime);
            }

            if (_snakesDict.ContainsKey(placeAfterRoll))
            {
                newPlace = _snakesDict[placeAfterRoll];
                Vector3 nextPosition = _board.TilePosition(newPlace);
                await MoveTransformAsync(playerTr, nextPosition, _hopTime);
                await DelayForSecounds(_pauseHopTime);
            }

            if (_laddersDict.ContainsKey(placeAfterRoll))
            {
                newPlace = _laddersDict[placeAfterRoll];
                Vector3 nextPosition = _board.TilePosition(newPlace);
                await MoveTransformAsync(playerTr, nextPosition, _hopTime);
                await DelayForSecounds(_pauseHopTime);
            }

            currentPlayer.placeOnBoard = newPlace;
        }

        public async Task MoveTransformAsync(Transform transform, Vector3 newPosition, float travelTime)
        {
            float elapsed = 0;
            Vector3 previousPosition = transform.position;

            while (elapsed < travelTime)
            {
                transform.position = Vector3.Lerp(previousPosition, newPosition, elapsed / travelTime);
                elapsed += Time.deltaTime;
                await Task.Yield();
            }

            transform.position = newPosition;
        }

        public async Task DelayForSecounds(float secounds)
        {
            await Task.Delay(Mathf.RoundToInt(secounds * 1000));
        }

        public Player CyclePlayerTurn()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _playerCounter = (_playerCounter + 1) % _players.Length;

                if (!IsAtEndOfBoard(_players[_playerCounter]))
                {
                    return _players[_playerCounter];
                }
            }

            Debug.Log("No available players that can move!");
            return null;
        }

        public int TotalTiles()
        {
            return _rows * _collumns;
        }

        public bool IsAtEndOfBoard(Player player)
        {
            return player.placeOnBoard == TotalTiles();
        }
    }
}