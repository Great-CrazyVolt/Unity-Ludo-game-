using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public int numberOfStepsToMove;
    public RollingDice rollingDice;
    public bool canPlayerMove = true;

    List<PathPoint> PlayerOnPathPointList = new List<PathPoint>();

    public bool canDiceRoll = true;
    public bool transferDice = false;
    public List<RollingDice> rollingDiceList;

    public int blueOutOfHome;
    public int greenOutOfHome;
    public int yellowOutOfHome;
    public int redOutOfHome;

    public int numberOfBlueCompletePlayers;
    public int numberOfGreenCompletePlayer;
    public int numberOfYellowCompletePlayers;
    public int numberOfRedCompletePlayers;

    public List<PlayerPiece> bluePlayerPieces;
    public List<PlayerPiece> redPlayerPieces;
    public List<PlayerPiece> greenPlayerPieces;
    public List<PlayerPiece> yellowPlayerPieces;

    // public List<PlayerPiece> AllPlayerPieces;

    public int totalPlayersCanPlay;
    public List<GameObject> playersHome;
    public int TotalSix;

    public List<int> indexOfEachPlayerPiece = new List<int>();

    // to track which player is rolling the dice
    public int playerNumber = 0;

    private void Awake()
    {
        gm = this;

        for (int i = 0; i < 4; i++)
        {
            indexOfEachPlayerPiece.Add(0); // Set initial index to 0 for each player
        }
    }

    public void AddPathPoint(PathPoint pathPoint)
    {
        PlayerOnPathPointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
    {
        if (PlayerOnPathPointList.Contains(pathPoint))
        {
            PlayerOnPathPointList.Remove(pathPoint);
        }
    }

    void roll()
    {
        rollingDiceList[2].MouseRoll();

    }
    public void transferRollingDice()
    {
        if (transferDice)
        {
            TotalSix = 0;
            transferDiceAccordingly();
        }

        else
        {
            if (GameManager.gm.totalPlayersCanPlay == 1)
            {
                if (GameManager.gm.rollingDice == GameManager.gm.rollingDiceList[2])
                {
                    Invoke("roll", 0.6f);
                }
            }
        }

        canDiceRoll = true;
        transferDice = false;
    }

    public void transferDiceAccordingly()
    {
        // Computer -> BOT
        if (GameManager.gm.totalPlayersCanPlay == 1)
        {
            if (rollingDice == rollingDiceList[0])
            {
                rollingDiceList[0].gameObject.SetActive(false);
                rollingDiceList[2].gameObject.SetActive(true);
                Invoke("roll", 0.6f);
            }
            else
            {
                rollingDiceList[2].gameObject.SetActive(false);
                rollingDiceList[0].gameObject.SetActive(true);
            }
        }
        else if (GameManager.gm.totalPlayersCanPlay == 2)
        {
            if (rollingDice == rollingDiceList[0])
            {
                rollingDiceList[0].gameObject.SetActive(false);
                rollingDiceList[2].gameObject.SetActive(true);
            }
            else
            {
                rollingDiceList[2].gameObject.SetActive(false);
                rollingDiceList[0].gameObject.SetActive(true);
            }
        }
        else if (GameManager.gm.totalPlayersCanPlay == 3)
        {
            int nextDice;

            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }

                // For players who still in the game, after some player won the match
                i = passOut(i);

                if (rollingDice == rollingDiceList[i])
                {
                    rollingDiceList[i].gameObject.SetActive(false);
                    rollingDiceList[nextDice].gameObject.SetActive(true);
                }
            }
        }
        else if (GameManager.gm.totalPlayersCanPlay == 4)
        {
            int nextDice;

            for (int i = 0; i < rollingDiceList.Count; i++)
            {
                if (i == (rollingDiceList.Count - 1))
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }
                // For players who still in the game, after some player won the match
                i = passOut(i);

                if (rollingDice == rollingDiceList[i])
                {
                    rollingDiceList[i].gameObject.SetActive(false);
                    rollingDiceList[nextDice].gameObject.SetActive(true);
                }
            }
        }
    }

    int passOut(int i)
    {
        if (i == 0)
        {
            if (redOutOfHome == 4)
            {
                return i + 1;
            }
        }
        else if (i == 1)
        {
            if (greenOutOfHome == 4)
            {
                return i + 1;
            }
        }
        else if (i == 2)
        {
            if (blueOutOfHome == 4)
            {
                return i + 1;
            }
        }
        else if (i == 3)
        {
            if (yellowOutOfHome == 4)
            {
                return i + 1;
            }
        }

        return i;
    }
}
