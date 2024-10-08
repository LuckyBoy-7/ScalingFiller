using System.Collections.Generic;
using Lucky.Managers.ObjectPool_;
using UnityEngine;

namespace Commands
{
    public class PutSquareCommand : ICommand
    {
        private Cell cell;

        public PutSquareCommand(Cell cell)
        {
            this.cell = cell;
        }

        public void Do()
        {
            cell.square = ObjectPoolManager.Instance.Get<Square>();
            var sr = cell.square.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.1f, 0.8f, 0.8f, 0.8f);
            cell.square.transform.position = cell.transform.position;
            cell.square.transform.localScale = Vector3.one * 50f;
            GameManager.Instance.squares.Add(cell.square);

            ObjectPoolManager.Instance.Release(cell.currentGhostSquare);
            cell.currentGhostSquare = null;
        }

        public void UnDo()
        {
            ObjectPoolManager.Instance.Release(cell.square);
            GameManager.Instance.squares.Remove(cell.square);
            cell.square = null;
        }
    }
}