using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffect : EventEffect
{
    public Image imageComponent;
    public SpriteRenderer spriteRenderer;
    private Color color;

    private bool isActive = false;
    public float effectSpeed = 1;
    private float cutoffAlpha = 0.1f;
    public Color transparentColor = new Color(1,1,1,0);
    public Color effectColor = new Color(1,0,0,1);

    protected override void DoEffect()
    {
        color = effectColor;
        isActive = true;
    }

    public void Update()
    {
        if(isActive)
        {
          color = Color.Lerp(color, transparentColor, effectSpeed * Time.deltaTime);
          if(color.a <= cutoffAlpha)
          {
              ClearEffect();
          }
          SetColor();
        }
    }

    void ClearEffect()
    {
        color = transparentColor;
        isActive = false;
    }

    void SetColor()
    {
        if(imageComponent != null){imageComponent.color = color;}
        if(spriteRenderer != null){spriteRenderer.color = color;}
    }
}
