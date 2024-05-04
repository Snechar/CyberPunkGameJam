using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialsManager : MonoBehaviour
{

    public TMP_Text label;

    public void SetLabel(string newVal) {
        label.text = newVal;
    }
}
