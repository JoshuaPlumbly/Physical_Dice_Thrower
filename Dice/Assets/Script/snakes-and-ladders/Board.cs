using UnityEngine;

namespace SnakesAndLadders
{
    public class Board : MonoBehaviour
    {
        public Vector3 _offset;
        public int _rows;
        public int _columns;
        public float _tileWidth;
        public float _tileHeight;

        public Vector3 TilePosition(int distanceAlongRow, int collumn)
        {
            float x = _tileWidth * distanceAlongRow + (_tileWidth * 0.5f);
            x = collumn % 2 == 0 ? x : _tileWidth * _rows - x;
            float z = _tileHeight * collumn + (_tileHeight * 0.5f);
            Vector3 fromOrigin = new Vector3(x, 0, z);
            return transform.position + _offset + fromOrigin;
        }

        public Vector3 TilePosition(int tile)
        {
            tile--;
            int x = tile % _rows;
            int y = tile / _rows;

            return TilePosition(x, y);
        }
    }
}
