using System;
using Commands;
using Lucky.Extensions;
using Lucky.Interactive;
using Lucky.Managers.ObjectPool_;
using UnityEditor;
using UnityEngine;

public class Cell : Interactable
{
    public bool isOccupied => square is not null;
    public Square currentGhostSquare = null;
    public Square square = null;


    protected override void OnCursorHover()
    {
        base.OnCursorHover();
        // 该格已经有东西了
        if (isOccupied)
        {
            var sr0 = square.GetComponent<SpriteRenderer>();
            sr0.color = new Color(0.8f, 0.1f, 0.1f, 0.6f);
            return;
        }

        if (!currentGhostSquare)
            currentGhostSquare = ObjectPoolManager.Instance.Get<Square>();
        var sr = currentGhostSquare.GetComponent<SpriteRenderer>();
        sr.color = new Color(0.1f, 0.8f, 0.1f, 0.6f);
        currentGhostSquare.transform.position = transform.position;
        currentGhostSquare.transform.localScale = Vector3.one * 50f;
    }

    protected override void OnCursorExit()
    {
        base.OnCursorExit();
        if (isOccupied)
        {
            var sr = square.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.1f, 0.8f, 0.8f, 0.8f);
        }
        else
        {
            ObjectPoolManager.Instance.Release(currentGhostSquare);
            currentGhostSquare = null;
        }
    }

    protected override void OnCursorPress()
    {
        base.OnCursorPress();
        if (GameManager.Instance.Steps > 0)
            if (isOccupied)
            {
                CommandManager.Instance.AddCommandSequence();
                ICommand command = new RemoveSquareCommand(this);
                CommandManager.Instance.AddCommand(command);
                command.Do();
            }
            else
            {
                CommandManager.Instance.AddCommandSequence();
                ICommand command = new PutSquareCommand(this);
                CommandManager.Instance.AddCommand(command);
                command.Do();
            }
    }
}