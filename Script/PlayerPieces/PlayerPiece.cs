using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public bool moveNow;
    public bool isReady;
    public int numberOfStepsToMove;
    public int numberOfStepAlreadyMoved;

    Coroutine playerMovement;

    public PathObjectParent pathParent;

    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;

    private void Awake()
    {
        pathParent = FindAnyObjectByType<PathObjectParent>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void makePlayerReadyToMove(PathPoint[] pathParent_)
    {
        isReady = true;
        transform.position = pathParent_[0].transform.position;
        numberOfStepAlreadyMoved = 1;

        GameManager.gm.numberOfStepsToMove = 0;

        previousPathPoint = pathParent_[0];
        currentPathPoint = pathParent_[0];

        currentPathPoint.AddPlayerPiece(this);

        GameManager.gm.AddPathPoint(currentPathPoint);

        GameManager.gm.transferRollingDice();

        return;

    }

    public void MovePlayer(PathPoint[] pathParent_)
    {
        playerMovement = StartCoroutine(MoveStep_enum(pathParent_));
    }

    IEnumerator MoveStep_enum(PathPoint[] pathParent_)
    {
        yield return new WaitForSeconds(0.25f);

        numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;

        for (int i = numberOfStepAlreadyMoved; i < (numberOfStepAlreadyMoved + numberOfStepsToMove); i++)
        {
            if (isPathAvailableToMove(numberOfStepsToMove, numberOfStepAlreadyMoved, pathParent_))
            {

                transform.position = pathParent_[i].transform.position;

                yield return new WaitForSeconds(0.35f);
            }
        }

        if (isPathAvailableToMove(numberOfStepsToMove, numberOfStepAlreadyMoved, pathParent_))
        {
            numberOfStepAlreadyMoved += numberOfStepsToMove;

            GameManager.gm.RemovePathPoint(previousPathPoint);
            previousPathPoint.RemovePlayerPiece(this);

            currentPathPoint = pathParent_[numberOfStepAlreadyMoved - 1];
            bool transfer = currentPathPoint.AddPlayerPiece(this);
            
            currentPathPoint.RescaleAndRepositioning();

            GameManager.gm.AddPathPoint(currentPathPoint);

            previousPathPoint = currentPathPoint;

            if (transfer && GameManager.gm.numberOfStepsToMove != 6)
            {
                GameManager.gm.transferDice = true;
            }

            GameManager.gm.numberOfStepsToMove = 0;
        }

        GameManager.gm.canPlayerMove = true;

        GameManager.gm.transferRollingDice();

        if (playerMovement != null)
        {
            StopCoroutine("MoveStep_enum");
        }
    }

    public bool isPathAvailableToMove(int numberOfStepsToMove, int numberOfStepAlreadyMoved, PathPoint[] pathParent_)
    {
        if (numberOfStepsToMove == 0)
        {
            return false;
        }

        int leftNumberOfPath = pathParent_.Length - numberOfStepAlreadyMoved;

        if (leftNumberOfPath >= numberOfStepsToMove)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
