using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Lucky.Extensions;
using UnityEngine;

public class SquarePosMark : MonoBehaviour
{
    private void Start()
    {
        this.CreateFuncTimer(
            () =>
            {
                var cell = Physics2D.OverlapPoint(transform.position)?.GetComponent<Cell>();
                new PutSquareCommand(cell).Do();
            }, () => 0.05f, isOneShot: true
        );
    }
}