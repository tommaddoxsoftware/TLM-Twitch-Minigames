using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TwitchIRC))]
public class GameJoin : MonoBehaviour {
    public int playerCount = 4;
    public int maxMessages = 100;
    public bool requireJoin; 

    private TwitchIRC m_IRC;
    private LinkedList<GameObject> m_messages = new LinkedList<GameObject>();
    private int randomNum;

    [SerializeField]
    private string[] m_players;

    // Use this for initialization
    void Start() {
        //m_IRC = this.GetComponent<TwitchIRC>();
        //m_IRC.messageRecievedEvent.AddListener(OnChatMsgRecieved);

        m_players = new string[playerCount];
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i] = null;
        }
    }

    public void Commands(string[] msg, string user)
    {
        //Attempts to join the game
        if (msg[0].ToLower() == "!join")
        {
            int index = CheckIfPlayer(user);
            int spaceIndex = CheckIfSpace();

            if (index == -1 && spaceIndex != -1)
            {
                Debug.Log(user + " :: has joined");
                AddPlayer(user, spaceIndex);
            }
        }


        if (msg[0].ToLower() == "!leave")
        {
            int index = CheckIfPlayer(user);

            if (index != -1)
            {
                Debug.Log(user + " :: has left");
                RemovePlayer(index);
            }
        }
    }

    private void AddPlayer(string player, int index)
    {
        Debug.Log("Adding Player: " + player);
        m_players[index] = player;
    }

    public void RemovePlayer(int index)
    {
        Debug.Log("Removing Player: " + m_players[index]);
        m_players[index] = null;
    }

    private int CheckIfSpace()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_players[i] == null) //Checks if there are empty spaces in the player list
            {
                return i; //Returns the index of an avalible space
            }
        }

        return -1; //Returns -1 if no spaces are found
    }

    public int CheckIfPlayer(string player)
    {
        if (requireJoin == false)
        {
            for (int i = 0; i < m_players.Length; i++)
            {
                if (player == m_players[i]) //Checks if the player is in the joined list
                {
                    return i; //Returns the index of the player if they are found
                }
            }

            return -1; //Returns -1 if no player is in the joined list
        }

        return 0;
    }


    public int MaxPlayers {
        get { return m_players.Length; }      
    }

}

