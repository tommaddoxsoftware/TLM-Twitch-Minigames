using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TwitchIRC))]
public class GameJoin : MonoBehaviour {
    public int playerCount = 4;
    public int maxMessages = 100;
    public bool requireJoin = true; 

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
        SendJoin(index);
    }

    public void RemovePlayer(int index)
    {
        Debug.Log("Removing Player: " + m_players[index]);
        m_players[index] = null;
        SendLeft(index);
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
        //if (requireJoin == true)
        //{
            for (int i = 0; i < m_players.Length; i++)
            {
                if (player == m_players[i]) //Checks if the player is in the joined list
                {
                    return i; //Returns the index of the player if they are found
                }
            }

            return -1; //Returns -1 if no player is in the joined list
        //}

        //return 0;
    }

    //Send join message to game controllers
    private void SendJoin(int index)
    {
        //Sends messages to minigame controllers to say that a player has joined
        try
        {
            this.GetComponent<BallControl>().PlayerJoined(index); //MiniGolf
        }
        catch { }
    }

    private void SendLeft(int index)
    {
        //Sends messages to minigame controllers to say that a player has left
        try
        {
            this.GetComponent<BallControl>().PlayerLeft(index); //MiniGolf
        }
        catch { }
    }

    public List<int> GetActivePlayers()
    {
        List<int> activePlayers = new List<int>();

        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_players[i] != null)
            {
                activePlayers.Add(i);
            }
        }

        return activePlayers;
    }

    public int MaxPlayers {
        get { return m_players.Length; }      
    }

}

