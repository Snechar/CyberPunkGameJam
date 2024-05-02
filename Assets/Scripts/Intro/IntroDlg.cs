using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class IntroDlg : MonoBehaviour, IIntroElement {
    public int DelayPostShowMS = 1500;
    public bool WaitForContinue = false;
    public bool ClearPrevious = false;

    private bool hasPlayed = false;

    public void Play() {
        gameObject.SetActive(true);
        hasPlayed = true;
    }

    public bool IsDone(int hasWaitedMS) {
        return hasPlayed && hasWaitedMS >= DelayPostShowMS;
    }

    public bool WaitsForInput() {
        return WaitForContinue;
    }

    public bool ShouldDeactivatePrevious() {
        return ClearPrevious;
    }

}

public interface IIntroElement {
    // makes the thing take its action
    public void Play();
    
    // is the thing done playing
    public bool IsDone(int hasWaitedMS);

    public bool WaitsForInput();

    public bool ShouldDeactivatePrevious();
}