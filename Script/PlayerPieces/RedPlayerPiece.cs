using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerPiece : PlayerPiece
{

    RollingDice RedHomeRollingDice;

    // Start is called before the first frame update
    void Start()
    {
        RedHomeRollingDice = GetComponentInParent<RedHome>().rollingDice;
    }

    public void OnMouseDown()
    {
        if (GameManager.gm.rollingDice != null)
        {
            if (!isReady)
            {
                if (GameManager.gm.rollingDice == RedHomeRollingDice && GameManager.gm.numberOfStepsToMove == 6 && GameManager.gm.canPlayerMove)
                {
                    GameManager.gm.redOutOfHome++;
                    makePlayerReadyToMove(pathParent.RedPathPoint);
                    GameManager.gm.numberOfStepsToMove = 0;
                    return;
                }
            }
            if (GameManager.gm.rollingDice == RedHomeRollingDice && isReady && GameManager.gm.canPlayerMove)
            {
                GameManager.gm.canPlayerMove = false;
                MovePlayer(pathParent.RedPathPoint);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}
