using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    private static List<int> recommendedLevels = new List<int>() { 1, 2, 3 };
    private static List<int> hardLevels = new List<int>() { };

    private void Start()
    {
        Button buttonPrefab = Resources.Load<Button>("LevelButton");
        for (int i = 1; i <= LevelManager.Instance.levelNumber; i++)
        {
            var button = Instantiate(buttonPrefab, transform);
            button.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
            int j = i;
            button.onClick.AddListener(() => { LevelManager.Instance.LoadLevel(j); });

            var image = button.GetComponent<Image>();
            if (recommendedLevels.Contains(i))
                image.color = Color.green;
            else if (hardLevels.Contains(i))
                image.color = Color.red;
        }
    }
}