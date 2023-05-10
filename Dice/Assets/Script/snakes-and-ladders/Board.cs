using System.Collections;
using System.Collections.Generic;
using Plumbly.Grid;
using UnityEngine;

namespace SnakesAndLadders
{
    public class Board : MonoBehaviour
    {
        public Vector3 _offset;
        public int _rows;
        public int _collums;
        public float _tileWidth;
        public float _tileHeight;

        public Vector3 TilePosition(int row, int distanceAlongColumn)
        {
            float x = _tileWidth * row + (_tileWidth * 0.5f);
            x = distanceAlongColumn % 2 == 0 ? x : _tileWidth * _rows - x;
            Vector3 fromOrigin = new Vector3(x, 0, _tileHeight * distanceAlongColumn + (_tileHeight * 0.5f));
            return transform.position + _offset + fromOrigin;
        }

        public Vector3 TilePosition(int tile)
        {
            int row = tile % _rows;
            int distanceAlongColumn = tile / _rows;

            return TilePosition(row, distanceAlongColumn);
        }
    }
}
