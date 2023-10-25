using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] SpriteRenderer numberSpriteHolder;
    [SerializeField] SpriteRenderer rollingDiceAnimation;
    [SerializeField] int numberGot;

    Coroutine generateRandomNumberDice;
    int numberOfPlayersOutFromHome;
    List<PlayerPiece> playerPieces;
    PathPoint[] currentPathPoint;
    public PlayerPiece currentPlayerPiece;

    public PathObjectParent pathObjectParent;

    List<int> redPlayerAtIndex = new List<int>();

    void Start()
    {
        pathObjectParent = FindObjectOfType<PathObjectParent>();
        if (pathObjectParent == null)
        {
            Debug.LogError("PathObjectParent component not found in the parent objects.");
        }
    }


    // Update is called once per frame
    public void OnMouseDown()
    {
        generateRandomNumberDice = StartCoroutine(rollDice());
    }

    public void MouseRoll()
    {
        generateRandomNumberDice = StartCoroutine(rollDice());
    }

    IEnumerator rollDice()
    {
        if (GameManager.gm.canDiceRoll)
        {
            yield return new WaitForEndOfFrame();

            GameManager.gm.canDiceRoll = false;

            numberSpriteHolder.gameObject.SetActive(false);
            rollingDiceAnimation.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.8f);

            // So, if the player got Six 2 times, than next time he wont get six
            int maximum = 6;
            if (GameManager.gm.TotalSix == 2)
            {
                maximum = 5;
                GameManager.gm.TotalSix = 0;
            }

            numberGot = Random.Range(0, maximum);
            if (numberGot == 5)
            {
                GameManager.gm.TotalSix += 1;
            }

            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot++;

            GameManager.gm.numberOfStepsToMove = numberGot;
            GameManager.gm.rollingDice = this;

            numberSpriteHolder.gameObject.SetActive(true);
            rollingDiceAnimation.gameObject.SetActive(false);

            outPlayers();

            // Computer -> bot
            if (GameManager.gm.totalPlayersCanPlay == 1 && GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[2])
            {
                if (GameManager.gm.numberOfStepsToMove == 6 && GameManager.gm.blueOutOfHome < 4)
                {
                    readyToMove(GameManager.gm.blueOutOfHome);
                }
                else if (playerCanMove())
                {
                    // At which index which player is
                    GameManager.gm.indexOfEachPlayerPiece[GameManager.gm.playerNumber] += numberGot;
                    currentPlayerPiece.MovePlayer(currentPathPoint);
                }
                else
                {
                    if (GameManager.gm.numberOfStepsToMove != 6 && numberOfPlayersOutFromHome == 0)
                    {
                        yield return new WaitForSeconds(0.5f);

                        GameManager.gm.transferDice = true;
                        GameManager.gm.transferRollingDice();
                    }
                }
            }

            else if (playerCanMove())
            {
                if (numberOfPlayersOutFromHome == 0)
                {
                    readyToMove(0);
                }
                else
                {
                    // At which index which player is
                    GameManager.gm.indexOfEachPlayerPiece[GameManager.gm.playerNumber] += numberGot;
                    currentPlayerPiece.MovePlayer(currentPathPoint);
                }
            }
            else
            {
                if (GameManager.gm.numberOfStepsToMove != 6 && numberOfPlayersOutFromHome == 0)
                {
                    yield return new WaitForSeconds(0.5f);

                    GameManager.gm.transferDice = true;
                    GameManager.gm.transferRollingDice();
                }
            }


            if (generateRandomNumberDice != null)
            {
                StopCoroutine(rollDice());
            }
        }
    }

    public void outPlayers()
    {
        if (GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[0])
        {
            playerPieces = GameManager.gm.redPlayerPieces;
            currentPathPoint = playerPieces[0].pathParent.RedPathPoint;
            numberOfPlayersOutFromHome = GameManager.gm.redOutOfHome;

            GameManager.gm.playerNumber = 0;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[1])
        {
            playerPieces = GameManager.gm.greenPlayerPieces;
            currentPathPoint = playerPieces[0].pathParent.GreenPathPoint;
            numberOfPlayersOutFromHome = GameManager.gm.greenOutOfHome;

            GameManager.gm.playerNumber = 1;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[2])
        {
            playerPieces = GameManager.gm.bluePlayerPieces;
            currentPathPoint = playerPieces[0].pathParent.BluePathPoint;
            numberOfPlayersOutFromHome = GameManager.gm.blueOutOfHome;

            GameManager.gm.playerNumber = 2;
        }
        else
        {
            playerPieces = GameManager.gm.yellowPlayerPieces;
            currentPathPoint = playerPieces[0].pathParent.YellowPathPoint;
            numberOfPlayersOutFromHome = GameManager.gm.yellowOutOfHome;

            GameManager.gm.playerNumber = 3;
        }
    }

    public bool playerCanMove()
    {
        // Computer -> BOT
        if (GameManager.gm.totalPlayersCanPlay == 1 && GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[2])
        {
            if (numberOfPlayersOutFromHome > 0)
            {
                GetPlayerIndex();

                // This function is doing is: gets the player piece
                // and check the available path, if present, then move the first piece
                // otherwise get the second piece, check condition and move it.
                // Like: if the number of player pieces out of home are 2
                // and the computer got 6, it will move the first player piece.

                // for (int i = 0; i < playerPieces.Count; i++)
                // {
                //     if (playerPieces[i].isReady)
                //     {
                //         for (int j = 0; j < pathObjectParent.CommonPathPoint.Length; j++)
                //         {
                //             if (playerPieces[i].transform.position == pathObjectParent.CommonPathPoint[j].transform.position)
                //             {
                //                 var killIndex = GameManager.gm.numberOfStepsToMove + j;

                //                 Debug.Log("killIndex: " + killIndex);

                //                 if (redPlayerAtIndex.Contains(killIndex))
                //                 {
                //                     currentPlayerPiece = playerPieces[i];
                //                     return true;
                //                 }
                //             }
                //         }
                //     }
                // }

                for (int i = 0; i < playerPieces.Count; i++)
                {
                    if (playerPieces[i].isReady)
                    {
                        if (canBotKillPiece(playerPieces[i]))
                        {
                            currentPlayerPiece = playerPieces[i];
                            return true;
                        }

                        else if (playerPieces[i].isPathAvailableToMove(GameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepAlreadyMoved, currentPathPoint))
                        {
                            currentPlayerPiece = playerPieces[i];
                            return true;
                        }
                    }
                }
            }
        }

        // 1 player out from home and should not have a 6 on dice, because user can unlock a new dice
        if (numberOfPlayersOutFromHome == 1 && GameManager.gm.numberOfStepsToMove != 6)
        {

            for (int i = 0; i < playerPieces.Count; i++)
            {
                if (playerPieces[i].isReady)
                {
                    if (playerPieces[i].isPathAvailableToMove(GameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepAlreadyMoved, currentPathPoint))
                    {
                        currentPlayerPiece = playerPieces[i];
                        return true;
                    }
                }
            }
        }
        else if (numberOfPlayersOutFromHome == 0 && GameManager.gm.numberOfStepsToMove == 6)
        {
            return true;
        }

        // for (int i = 0; i < GameManager.gm.indexOfEachPlayerPiece.Count; i++)
        // {
        //     // Set initial index to 0 for each player
        //     Debug.Log(GameManager.gm.indexOfEachPlayerPiece.Count);
        // }

        return false;
    }

    bool canBotKillPiece(PlayerPiece playerPiece_)
    {
        for (int j = 0; j < pathObjectParent.CommonPathPoint.Length; j++)
        {
            if (playerPiece_.transform.position == pathObjectParent.CommonPathPoint[j].transform.position)
            {
                var killIndex = GameManager.gm.numberOfStepsToMove + j;

                Debug.Log("killIndex: " + killIndex);

                if (redPlayerAtIndex.Contains(killIndex))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void GetPlayerIndex()
    {
        // if (pathObjectParent == null)
        // {
        //     Debug.LogError("PathObjectParent is null. Make sure it is assigned in the inspector.");
        //     return -1;
        // }

        for (int i = 0; i < GameManager.gm.redPlayerPieces.Count; i++)
        {
            for (int j = 0; j < pathObjectParent.CommonPathPoint.Length; j++)
            {
                if (GameManager.gm.redPlayerPieces[i].transform.position == pathObjectParent.CommonPathPoint[j].transform.position)
                {
                    redPlayerAtIndex.Add(j);
                }
            }
        } // If player position not found in CommonPathPoint array
    }

    void readyToMove(int index)
    {
        if (GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[0])
        {
            GameManager.gm.redOutOfHome += 1;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[1])
        {
            GameManager.gm.greenOutOfHome += 1;
        }
        else if (GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[2])
        {
            GameManager.gm.blueOutOfHome += 1;
        }
        else
        {
            GameManager.gm.yellowOutOfHome += 1;
        }

        playerPieces[index].makePlayerReadyToMove(currentPathPoint);
    }
}
