using System;
using Lucky.Managers.ObjectPool_;using UnityEngine;

public class Square : MonoBehaviour, IRecycle
{
    public void OnGet()
    {
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }
}