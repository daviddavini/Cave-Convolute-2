using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{

    private Health health = null;
    public int numberOfFullHearts;
    public int numberOfHearts;

    public GameObject emptyHearts;
    public GameObject fullHearts;
    public Vector2 heartImageDimensions;

    void Awake()
    {
        health = gameObject.GetComponentInParent<Health>();
        numberOfFullHearts = Mathf.CeilToInt(health.health);
        numberOfHearts = Mathf.CeilToInt(health.maxHealth);
        health.NewHealthEvent += UpdateAndRefresh;
        Refresh();
    }

    void UpdateAndRefresh(float healthAmt, float healthChange)
    {
        numberOfFullHearts = Mathf.CeilToInt(healthAmt);
        Refresh();
    }

    void Refresh()
    {
        SetSizeOfTiledUIImageObject(emptyHearts, new Vector2(numberOfHearts,1));
        SetSizeOfTiledUIImageObject(fullHearts, new Vector2(numberOfFullHearts,1));
        fullHearts.transform.localPosition =
          new Vector3((numberOfFullHearts-numberOfHearts)*0.5f*heartImageDimensions.x, 0, 0);
    }

    void SetSizeOfTiledUIImageObject(GameObject tiledUIImageObject, Vector2 size)
    {
        tiledUIImageObject.GetComponent<RectTransform>().sizeDelta =
          new Vector2(heartImageDimensions.x*size.x, heartImageDimensions.y*size.y);
    }
}
