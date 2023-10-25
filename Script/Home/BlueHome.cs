using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueHome : LudoHome
{
    RollingDice blueHomeRollingDice;
    // Start is called before the first frame update
    void Start()
    {
        blueHomeRollingDice = GetComponentInParent<BlueHome>().rollingDice;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
