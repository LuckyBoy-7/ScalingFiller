using Commands;
using Lucky.Extensions;
using Lucky.Interactive;
using Lucky.Managers.ObjectPool_;
using UnityEditor;
using UnityEngine;

public class Cell : Interactable
{
    public bool isOccupied = false;
    public Square currentGhostSquare = null;

    protected override void OnCursorHover()
    {
        base.OnCursorHover();
        // 该格已经有东西了
        if (isOccupied)
            return;
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
        if (currentGhostSquare)
        {
            ObjectPoolManager.Instance.Release(currentGhostSquare);
            currentGhostSquare = null;
        }
    }

    protected override void OnCursorPress()
    {
        base.OnCursorPress();
        if (currentGhostSquare)
        {
            CommandManager.Instance.AddCommandSequence();
            ICommand command = new PutSquareCommand(this);
            CommandManager.Instance.AddCommand(command);
            command.Do();
        }
    }
}