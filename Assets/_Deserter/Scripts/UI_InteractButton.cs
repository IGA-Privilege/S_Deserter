using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InteractButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;
    private float lightAlpha = 1.0f;
    private float darkAlpha = 0.4f;

    public void SetInteractable(bool canInteract)
    {
        if (canInteract)
        {
            button.interactable = true;
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, lightAlpha);
            iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, lightAlpha);
        }
        else
        {
            button.interactable = false;
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, darkAlpha);
            iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, darkAlpha);
        }
    }
}
