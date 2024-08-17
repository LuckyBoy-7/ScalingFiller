using System.Collections.Generic;
using Lucky.Managers.ObjectPool_;
using UnityEngine;

namespace Commands
{
    public class PutSquareCommand : ICommand
    {
        private Cell cell;
        private Square square;

        public PutSquareCommand(Cell cell)
        {
            this.cell = cell;
        }

        public void Do()
        {
            cell.isOccupied = true;
            square = ObjectPoolManager.Instance.Get<Square>();
            var sr = square.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.1f, 0.8f, 0.8f, 0.8f);
            square.transform.position = cell.transform.position;
            square.transform.localScale = Vector3.one * 50f;
            GameManager.Instance.squares.Add(square);

            ObjectPoolManager.Instance.Release(cell.currentGhostSquare);
            cell.currentGhostSquare = null;
        }

        public void UnDo()
        {
            ObjectPoolManager.Instance.Release(square);
            GameManager.Instance.squares.Remove(square);
            square = null;
        }
    }
}