using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyBarSwitch : MonoBehaviour {
    public void SwitchToBar() {
        Debug.Log("SwitchToBar called");
        Driver.GetInstance().SwitchScenes(JamScenes.BAR);
    }
}
