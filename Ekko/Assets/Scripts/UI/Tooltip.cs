using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text detailText;
    public Image displaySprite;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public void UpdateTooltip(string _detailText, Sprite _displaySprite)
    {
        detailText.text = _detailText;

        displaySprite.sprite = _displaySprite;
    }

    // public void StePosition(Vector2 _pos)
    // {
    //     transform.localPosition = _pos;
    // }
}
