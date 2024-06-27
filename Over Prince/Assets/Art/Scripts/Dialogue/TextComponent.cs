using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A class that should be used for Dialogue Text Components.
/// Really as it stands, this is currently just for referencing the Shadow text
/// since TextMeshProUGUI does not support changing the order of the text components.
/// </summary>
public class TextComponent : MonoBehaviour
{
    public TextMeshProUGUI shadowText;

    public void Start() {
        if (shadowText != null) {
        }
    }
}
