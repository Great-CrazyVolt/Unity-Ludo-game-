using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerPiece : PlayerPiece
{
    RollingDice blueHomeRollingDice;

    // Start is called before the first frame update
    void Start()
    {
        blueHomeRollingDice = GetComponentInParent<BlueHome>().rollingDice;
    }
    public void OnMouseDown()
    {
        if (GameManager.gm.rollingDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rollingDice == blueHomeRollingDice && GameManager.gm.numberOfStepsToMove == 6 && GameManager.gm.canPlayerMove)
                {
                    GameManager.gm.blueOutOfHome++;
                    makePlayerReadyToMove(pathParent.BluePathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }

            if (GameManager.gm.rollingDice == blueHomeRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                MovePlayer(pathParent.BluePathPoint);
            }
        }
    }
}
