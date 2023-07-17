using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode { Exploration, Dialogue };
public class ModeController : MonoBehaviour
{

    public Mode CurrentMode { get; private set; }

    private void Start()
    {
        CurrentMode = Mode.Exploration;
    }

    // Singleton
    public static ModeController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void SwitchMode(Mode targetMode)
    {
        CurrentMode = targetMode;
    }

}
