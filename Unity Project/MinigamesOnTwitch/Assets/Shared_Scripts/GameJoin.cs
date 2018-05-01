using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TwitchIRC))]
public class GameJoin : MonoBehaviour
{
    public int playerCount;
    public int maxMessages = 100;
    public bool requireJoin = true;

    private TwitchIRC m_irc;
    private LinkedList<GameObject> m_messages = new LinkedList<GameObject>();
    private int randomNum;

    [SerializeField]
    private string[] m_players;

    // Use this for initialization
    void Start()
    {
        m_irc = this.GetComponent<TwitchIRC>();

        //Get Player count from playerprefs (Set in game config on main menu)
        playerCount = PlayerPrefs.GetInt("MaxPlayers");
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
                //New player successfully joins
                Debug.Log(user + " :: has joined");
                m_irc.SendMsg(user + " has joined");
                AddPlayer(user, spaceIndex);
            }
            else if (index != -1)
            {
                //Player try to join again
                m_irc.SendMsg(user + " has already joined");
            }
            else if (spaceIndex == -1)
            {
                //No player spaces left
                m_irc.SendMsg(user + " no spaces left");
            }
        }


        if (msg[0].ToLower() == "!leave")
        {
            int index = CheckIfPlayer(user);

            if (index != -1)
            {
                //Player leaves
                Debug.Log(user + " :: has left");
                m_irc.SendMsg(user + " has left");
                RemovePlayer(index);
            }
            else
            {
                //Non player trys to leave
                m_irc.SendMsg(user + " is not playing");
            }
        }
    }

    private void AddPlayer(string player, int index)
    {
        m_players[index] = player;
        SendJoin(index);
    }

    public void RemovePlayer(int index)
    {
        m_players[index] = null;
        SendLeft(index);
    }

    private int CheckIfSpace()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            //Checks if there are empty spaces in the player list
            if (m_players[i] == null)
            {
                //Returns the index of an avalible space
                return i;
            }
        }

        return -1; //Returns -1 if no spaces are found
    }

    public int CheckIfPlayer(string player)
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            //Checks if the player is in the joined list
            if (player == m_players[i]) 
            {
                //Returns the index of the player if they are found
                return i; 
            }
        }

        //Returns -1 if no player is in the joined list
        return -1; 
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
            //MiniGolf
            this.GetComponent<BallControl>().PlayerLeft(index); 
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

    public int MaxPlayers
    {
        get { return m_players.Length; }
    }

}

