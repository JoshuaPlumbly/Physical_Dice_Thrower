using UnityEngine;

namespace SnakesAndLadders
{
    public class Player : MonoBehaviour
    {
        public int placeOnBoard;
        [SerializeField] Color _colour = new Color(1, 1, 1, 1);

        private void Awake()
        {
            SetColour(_colour);
        }

        public void SetColour(Color colour)
        {
            if (TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                _colour = colour;
                meshRenderer.material.color = colour;
            }
        }
    }
}
