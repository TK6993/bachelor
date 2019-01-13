using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpdater : MonoBehaviour {

    SpriteRenderer render;
    public Sprite[] levels;
    public int level = 0;
    int maxLevels = 0;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        render.sprite = levels[level];
        maxLevels = levels.Length-1;
    }

    private void OnMouseDown()
    {
        level = (level < maxLevels) ? level + 1 : level; 
        render.sprite = levels[level];
    }

}
