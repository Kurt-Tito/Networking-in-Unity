using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance { set; get; }

    int gameCount = 0;

    public bool isPlayerTurn = true;
    public string choice = "";
    public Text P1choiceText;
    public Text P2choiceText;

    public Button rock;
    public Button paper;
    public Button scissor;


    public bool isPlayer1;
    private Client client;

	// Use this for initialization
	void Start ()
    {
        Instance = this;

        client = FindObjectOfType<Client>();
        isPlayer1 = client.isHost;
        //isPlayerTurn = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        if (!isPlayerTurn)
            choiceText.text = "INVALID TURN";
        else
        {
            choiceText.text = choice;
        }
        */

        /*
        if (isPlayerTurn == true)
            choiceText.text = choice;
        */

        if (!isPlayerTurn)
        {
            rock.interactable = false;
            paper.interactable = false;
            scissor.interactable = false;
        }

    }

    private void chooseRock()
    {
        //if (isPlayer1)
        //    P1choiceText.text = "Rock";
        //else
        //    P2choiceText.text = "Rock";

        choice = "Rock";
        //isPlayerTurn = false;

        if (client.clientName == "Host")
            P1choiceText.text = choice;
        else
            P2choiceText.text = choice;
    }

    private void choosePaper()
    {
        //if (isPlayer1)
        //    P1choiceText.text = "Paper";
        //else
        //    P2choiceText.text = "Paper";

        choice = "Paper";
        //isPlayerTurn = false;

        if (client.clientName == "Host")
            P1choiceText.text = choice;
        else
            P2choiceText.text = choice;
    }

    private void chooseScissor()
    {
        //if (isPlayer1)
        //    P1choiceText.text = "Scissor";
        //else
        //    P2choiceText.text = "Scissor";

        choice = "Scissor";
        //isPlayerTurn = false;

        if (client.clientName == "Host")
            P1choiceText.text = choice;
        else
            P2choiceText.text = choice;
    }

    public void rockButton()
    {
        chooseRock();
        endTurn();
        //isPlayerTurn = false;
    }

    public void paperButton()
    {
        choosePaper();
        endTurn();
        //isPlayerTurn = false;
    }

    public void scissorButton()
    {
        chooseScissor();
        endTurn();
        //isPlayerTurn = false;
    }

    public void endTurn()
    {
        isPlayerTurn = false;
        string msg = "";

        if (isPlayer1)
            msg = "P1|";
        else
            msg = "P2|";

        //if (isPlayer1)
        //    msg += P1choiceText.text + "|";
        //else
        //    msg += P2choiceText.text + "|";

        msg += choice;

        //msg += ((isPlayer1) ? 1 : 0).ToString();
        client.Send(msg);
    }

    public void startTurn()
    {
        isPlayerTurn = true;
    }

    public void setChoice(string data, string isHost)
    {
        gameCount++;

        if (gameCount == 2)
        {
            if (client.clientName == "Host")
            {
                //P1choiceText.text = choice;
                P2choiceText.text = data;
            }
            else if (client.clientName == "Client")
            {
                P1choiceText.text = data;
                //P2choiceText.text = choice;
            }

            gameCount = 0;
        }
    }

    public void setChoice(string data)
    {
        //gameCount++;

        if (gameCount == 0)
        {
            if (client.clientName == "Host")
            {
                //P1choiceText.text = choice;
                P2choiceText.text = data;
            }
            else if (client.clientName == "Client")
            {
                P1choiceText.text = data;
                //P2choiceText.text = choice;
            }

            gameCount = 0;
        }
    }

    public void incrementCount()
    {
        gameCount++;
    }
}
