using System;
using System.Collections;
using System.Collections.Generic;
using Lucky.Extensions;
using Lucky.Managers;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    private List<List<ICommand>> commandSequences = new();

    public void AddCommandSequence()
    {
        while (commandSequences.Count > idx + 1)
            commandSequences.Pop(-1);
        commandSequences.Add(new());
        idx += 1;
    }

    public void AddCommand(ICommand command) => commandSequences[^1].Add(command);
    public void RemoveLastCommandSequence() => commandSequences.Pop(-1);

    private int idx = -1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            UnDo();
        else if (Input.GetKeyDown(KeyCode.X))
            Do();
    }

    private void Do()
    {
        if (commandSequences.Count - 1 > idx)
            idx += 1;
        else
        {
            print("Do到底了");
            return;
        }

        foreach (var command in commandSequences[idx])
            command.Do();
    }

    private void UnDo()
    {
        if (idx != -1)
        {
            foreach (var command in commandSequences[idx])
                command.UnDo();
            idx -= 1;
        }
        else
        {
            print("UnDo到底了");
        }
    }

}