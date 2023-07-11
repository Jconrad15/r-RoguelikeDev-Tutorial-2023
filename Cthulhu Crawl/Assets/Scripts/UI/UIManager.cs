using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init()
    {
        FindAnyObjectByType<HealthUI>().Init();
    }


}
