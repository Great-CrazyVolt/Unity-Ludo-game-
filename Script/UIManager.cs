using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject gamePanel;

    public void Game1()
    {
        GameManager.gm.totalPlayersCanPlay = 2;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);

        GameManager.gm.playersHome[1].SetActive(false);
        GameManager.gm.playersHome[3].SetActive(false);
    }

    public void Game2()
    {
        GameManager.gm.totalPlayersCanPlay = 3;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);

        GameManager.gm.playersHome[3].SetActive(false);
    }

    public void Game3()
    {
        GameManager.gm.totalPlayersCanPlay = 4;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void Game4()
    {
        // Computer -> BOT
        GameManager.gm.totalPlayersCanPlay = 1;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);

        GameManager.gm.playersHome[1].SetActive(false);
        GameManager.gm.playersHome[3].SetActive(false);
    }
}
