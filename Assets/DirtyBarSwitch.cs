using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyBarSwitch : MonoBehaviour
{
    public void SwitchToBar()
    {
        Driver.GetInstance().SwitchScenes(JamScenes.BAR);
    }
}
