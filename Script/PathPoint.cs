using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public PathObjectParent pathObjectParent;
    public List<PlayerPiece> playerPieceList = new List<PlayerPiece>();
    PathPoint[] pathPointsToMoveOn;

    void Start()
    {
        pathObjectParent = GetComponentInParent<PathObjectParent>();
    }

    public bool AddPlayerPiece(PlayerPiece playerPiece_)
    {
        if (this.name == "CommonCenteralPoint")
        {
            addPlayer(playerPiece_);
            // when the player piece is at the center common point
            complete(playerPiece_);
            return false;

        }
        if (!pathObjectParent.SafePoints.Contains(this))
        {
            if (playerPieceList.Count == 1)
            {
                string prePlayerPieceName = playerPieceList[0].name;
                string currentPlayerPieceName = playerPiece_.name;
                currentPlayerPieceName = currentPlayerPieceName.Substring(0, currentPlayerPieceName.Length - 4);

                if (!prePlayerPieceName.Contains(currentPlayerPieceName))
                {
                    playerPieceList[0].isReady = false;
                    // on kill revert the player to its home
                    StartCoroutine(revertToStart(playerPieceList[0]));

                    playerPieceList[0].numberOfStepAlreadyMoved = 0;
                    RemovePlayerPiece(playerPieceList[0]);
                    playerPieceList.Add(playerPiece_);
                    return false;
                }
            }
        }
        addPlayer(playerPiece_);
        return true;
    }

    void addPlayer(PlayerPiece playerPiece_)
    {
        playerPieceList.Add(playerPiece_);
        RescaleAndRepositioning();
    }

    IEnumerator revertToStart(PlayerPiece playerPiece_)
    {
        if (playerPiece_.name.Contains("Blue"))
        {
            GameManager.gm.blueOutOfHome -= 1;
            pathPointsToMoveOn = pathObjectParent.BluePathPoint;
        }
        else if (playerPiece_.name.Contains("Red"))
        {
            GameManager.gm.redOutOfHome -= 1;
            pathPointsToMoveOn = pathObjectParent.RedPathPoint;

        }
        else if (playerPiece_.name.Contains("Yellow"))
        {
            GameManager.gm.yellowOutOfHome -= 1;
            pathPointsToMoveOn = pathObjectParent.YellowPathPoint;

        }
        else
        {
            GameManager.gm.greenOutOfHome -= 1;
            pathPointsToMoveOn = pathObjectParent.GreenPathPoint;

        }

        for (int i = playerPiece_.numberOfStepAlreadyMoved - 1; i >= 0; i--)
        {
            playerPiece_.transform.position = pathPointsToMoveOn[i].transform.position;
            yield return new WaitForSeconds(0.02f);
        }

        playerPiece_.transform.position = pathObjectParent.BasePoints[basePointPosition(playerPiece_.name)].transform.position;
    }

    int basePointPosition(string name)
    {
        for (int i = 0; i < pathObjectParent.BasePoints.Length; i++)
        {
            if (pathObjectParent.BasePoints[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    public void RemovePlayerPiece(PlayerPiece playerPiece_)
    {
        if (playerPieceList.Contains(playerPiece_))
        {
            playerPieceList.Remove(playerPiece_);
            RescaleAndRepositioning();
        }
    }

    void complete(PlayerPiece playerPiece_)
    {
        // For celebration
        int totalCompletePlayer;

        if (playerPiece_.name.Contains("Blue"))
        {
            GameManager.gm.blueOutOfHome -= 1;
            totalCompletePlayer = GameManager.gm.numberOfBlueCompletePlayers += 1;
        }
        else if (playerPiece_.name.Contains("Red"))
        {
            GameManager.gm.redOutOfHome -= 1;
            totalCompletePlayer = GameManager.gm.numberOfRedCompletePlayers += 1;
        }
        else if (playerPiece_.name.Contains("Yellow"))
        {
            GameManager.gm.yellowOutOfHome -= 1;
            totalCompletePlayer = GameManager.gm.numberOfYellowCompletePlayers += 1;
        }
        else
        {
            GameManager.gm.greenOutOfHome -= 1;
            totalCompletePlayer = GameManager.gm.numberOfGreenCompletePlayer += 1;
        }

        // for celebration
        if (totalCompletePlayer == 4)
        {

        }
    }

    public void RescaleAndRepositioning()
    {
        int playerCount = playerPieceList.Count;
        bool isOdd = playerCount % 2 == 0 ? false : true;

        int extent = playerCount / 2;
        int counter = 0;
        int SpriteLayer = 0;

        if (isOdd)
        {
            for (int i = -extent; i <= extent; i++)
            {
                playerPieceList[counter].transform.localScale = new Vector3(pathObjectParent.scales[playerCount - 1], pathObjectParent.scales[playerCount - 1], 1f);
                playerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[playerCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extent; i < extent; i++)
            {
                playerPieceList[counter].transform.localScale = new Vector3(pathObjectParent.scales[playerCount - 1], pathObjectParent.scales[playerCount - 1], 1f);
                playerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent.positionDifference[playerCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }

        for (int i = 0; i < playerPieceList.Count; i++)
        {
            playerPieceList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = SpriteLayer;
            SpriteLayer++;
        }
    }
}
