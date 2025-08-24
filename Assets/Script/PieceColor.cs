using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class PieceColor : MonoBehaviour
{
    public Color color;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
