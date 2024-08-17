using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Lucky.Collections;
using Lucky.Managers;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Square> squares = new();
    public const int Unit = 60;

    private void Update()
    {
        foreach (KeyCode key in new List<KeyCode> { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow })
        {
            if (Input.GetKeyDown(key))
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    Shrink(key);
                    break;
                }

                Inflate(key);
                break;
            }
        }

        // print(squares.Count);
    }

    private void Inflate(KeyCode inflateDir)
    {
        HashSet<Square> vis = new();
        HashSet<Vector2> newPoses = new();
        foreach (var square in squares)
        {
            if (vis.Contains(square))
                continue;

            #region 筛选每个连通块, 存放至tmpVis里

            HashSet<Square> tmpVis = new();
            Deque<Square> d = new Deque<Square>();
            d.Append(square);
            while (d.Count > 0)
            {
                Square tmp = d.PopLeft();
                if (tmpVis.Contains(tmp))
                    continue;
                tmpVis.Add(tmp);
                vis.Add(tmp);

                foreach (var dir in new List<Vector2> { new(Unit, 0), new(-Unit, 0), new(0, Unit), new(0, -Unit) })
                {
                    // 不要用square判断, 因为ghost也是square
                    Cell neighbourCell = Physics2D.OverlapPoint(tmp.transform.position + (Vector3)dir)?.GetComponent<Cell>();
                    if (neighbourCell && neighbourCell.isOccupied)
                        d.Append(neighbourCell.square);
                }
            }

            #endregion

            #region 遍历每个连通块, 根据缩放方向得出结果位置集合

            float minX = Single.PositiveInfinity;
            float minY = Single.PositiveInfinity;
            float maxX = Single.NegativeInfinity;
            float maxY = Single.NegativeInfinity;
            foreach (var sq in tmpVis)
            {
                minX = Mathf.Min(minX, sq.transform.position.x);
                minY = Mathf.Min(minY, sq.transform.position.y);
                maxX = Mathf.Max(maxX, sq.transform.position.x);
                maxY = Mathf.Max(maxY, sq.transform.position.y);
            }

            foreach (var sq in tmpVis)
            {
                Vector3 pos = sq.transform.position;
                Vector3 pos1, pos2;
                switch (inflateDir)
                {
                    case KeyCode.LeftArrow:
                        pos1 = new Vector2(maxX - (maxX - pos.x) * 2, pos.y);
                        pos2 = new Vector2(maxX - (maxX - pos.x) * 2 - Unit, pos.y);
                        break;
                    case KeyCode.RightArrow:
                        pos1 = new Vector2((pos.x - minX) * 2 + minX, pos.y);
                        pos2 = new Vector2((pos.x - minX) * 2 + minX + Unit, pos.y);
                        break;
                    case KeyCode.DownArrow:
                        pos1 = new Vector2(pos.x, maxY - (maxY - pos.y) * 2);
                        pos2 = new Vector2(pos.x, maxY - (maxY - pos.y) * 2 - Unit);
                        break;
                    case KeyCode.UpArrow:
                        pos1 = new Vector2(pos.x, (pos.y - minY) * 2 + minY);
                        pos2 = new Vector2(pos.x, (pos.y - minY) * 2 + minY + Unit);
                        break;
                    default:
                        throw new Exception("Impossible");
                }

                //
                // if (newPoses.Any(p => Vector3.Distance(p, pos1) < 0.1f))
                //     return;
                // if (newPoses.Any(p => Vector3.Distance(p, pos2) < 0.1f))
                //     return;
                if (newPoses.Contains(pos1) || newPoses.Contains(pos2))
                    return;
                newPoses.Add(pos1);
                newPoses.Add(pos2);
                // print(pos1);
                // print(pos2);
            }

            #endregion
        }

        #region 检查结果位置集合是否合法, 合法则清空当前square并放置新的square

        if (newPoses.Count > 0 && newPoses.All(pos => Physics2D.OverlapPoint(pos)?.GetComponent<Cell>()))
        {
            CommandManager.Instance.AddCommandSequence();
            foreach (var square in squares.ToList())
            {
                ICommand command = new RemoveSquareCommand(Physics2D.OverlapPoint(square.transform.position)?.GetComponent<Cell>());
                CommandManager.Instance.AddCommand(command);
                command.Do();
            }


            foreach (var newPos in newPoses)
            {
                Cell cell = Physics2D.OverlapPoint(newPos).GetComponent<Cell>();
                ICommand command = new PutSquareCommand(cell);
                CommandManager.Instance.AddCommand(command);
                command.Do();
            }
        }

        #endregion
    }

    private void Shrink(KeyCode shrinkDir)
    {
        HashSet<Square> vis = new();
        HashSet<Vector2> newPoses = new();
        foreach (var square in squares)
        {
            if (vis.Contains(square))
                continue;

            #region 筛选每个连通块, 存放至tmpVis里

            HashSet<Square> tmpVis = new();
            Deque<Square> d = new Deque<Square>();
            d.Append(square);
            while (d.Count > 0)
            {
                Square tmp = d.PopLeft();
                if (tmpVis.Contains(tmp))
                    continue;
                tmpVis.Add(tmp);
                vis.Add(tmp);

                foreach (var dir in new List<Vector2> { new(Unit, 0), new(-Unit, 0), new(0, Unit), new(0, -Unit) })
                {
                    // 不要用square判断, 因为ghost也是square
                    Cell neighbourCell = Physics2D.OverlapPoint(tmp.transform.position + (Vector3)dir)?.GetComponent<Cell>();
                    if (neighbourCell && neighbourCell.isOccupied)
                        d.Append(neighbourCell.square);
                }
            }

            #endregion

            #region 遍历每个连通块, 根据缩放方向得出结果位置集合

            float minX = Single.PositiveInfinity;
            float minY = Single.PositiveInfinity;
            float maxX = Single.NegativeInfinity;
            float maxY = Single.NegativeInfinity;
            foreach (var sq in tmpVis)
            {
                minX = Mathf.Min(minX, sq.transform.position.x);
                minY = Mathf.Min(minY, sq.transform.position.y);
                maxX = Mathf.Max(maxX, sq.transform.position.x);
                maxY = Mathf.Max(maxY, sq.transform.position.y);
            }

            HashSet<Vector3> repeatPoses = new();
            foreach (var sq in tmpVis)
            {
                Vector3 pos = sq.transform.position;
                Vector3 pos1, pos2;
                switch (shrinkDir)
                {
                    case KeyCode.LeftArrow:
                        pos1 = new Vector2((int)((pos.x - minX) / Unit) / 2 * Unit + minX, pos.y);
                        break;
                    case KeyCode.RightArrow:
                        pos1 = new Vector2(maxX - (int)((maxX - pos.x) / Unit) / 2 * Unit, pos.y);
                        break;
                    case KeyCode.DownArrow:
                        pos1 = new Vector2(pos.x, (int)((pos.y - minY) / Unit) / 2 * Unit + minY);
                        break;
                    case KeyCode.UpArrow:
                        pos1 = new Vector2(pos.x, maxY - (int)((maxY - pos.y) / Unit) / 2 * Unit);

                        break;
                    default:
                        throw new Exception("Impossible");
                }

                //
                // if (newPoses.Any(p => Vector3.Distance(p, pos1) < 0.1f))
                //     return;
                // if (newPoses.Any(p => Vector3.Distance(p, pos2) < 0.1f))
                //     return;
                if (repeatPoses.Contains(pos1))
                {
                    newPoses.Add(pos1);
                    repeatPoses.Remove(pos1);
                }
                else
                    repeatPoses.Add(pos1);
                // print(pos1);
                // print(pos2);
            }

            if (repeatPoses.Count > 0)
                return;

            #endregion
        }

        #region 检查结果位置集合是否合法, 合法则清空当前square并放置新的square

        if (newPoses.Count > 0 && newPoses.All(pos => Physics2D.OverlapPoint(pos)?.GetComponent<Cell>()))
        {
            CommandManager.Instance.AddCommandSequence();
            foreach (var square in squares.ToList())
            {
                ICommand command = new RemoveSquareCommand(Physics2D.OverlapPoint(square.transform.position)?.GetComponent<Cell>());
                CommandManager.Instance.AddCommand(command);
                command.Do();
            }


            foreach (var newPos in newPoses)
            {
                Cell cell = Physics2D.OverlapPoint(newPos).GetComponent<Cell>();
                ICommand command = new PutSquareCommand(cell);
                CommandManager.Instance.AddCommand(command);
                command.Do();
            }
        }

        #endregion
    }
}