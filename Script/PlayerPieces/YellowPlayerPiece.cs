using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerPiece : PlayerPiece
{
    RollingDice yellowHomeRollingDice;

    // Start is called before the first frame update
    void Start()
    {
        yellowHomeRollingDice = GetComponentInParent<YellowHome>().rollingDice;
    }
    public void OnMouseDown()
    {
        if (GameManager.gm.rollingDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rollingDice == yellowHomeRollingDice && GameManager.gm.numberOfStepsToMove == 6 && GameManager.gm.canPlayerMove)
                {
                    GameManager.gm.yellowOutOfHome++;
                    makePlayerReadyToMove(pathParent.YellowPathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rollingDice == yellowHomeRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                MovePlayer(pathParent.YellowPathPoint);
            }
        }

    }
}
