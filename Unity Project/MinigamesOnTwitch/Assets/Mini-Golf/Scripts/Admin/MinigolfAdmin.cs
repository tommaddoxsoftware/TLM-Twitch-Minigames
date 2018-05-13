using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdminControls)),
    RequireComponent(typeof(MinigolfController)),
    RequireComponent(typeof(GameJoin))]
public class MinigolfAdmin : MonoBehaviour
{
    private MinigolfController m_minigolf;
    private GameJoin m_players;
    private TwitchIRC m_irc;

    // Use this for initialization
    void Start()
    {
        m_minigolf = this.GetComponent<MinigolfController>();
        m_players = this.GetComponent<GameJoin>();
        m_irc = this.GetComponent<TwitchIRC>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Commands(string user, string[] msgArray)
    {


        switch (msgArray[0].ToLower())
        {
            case "!resetall":
                break;
            case "!reset":
                break;
            case "!nextcourse":
                break;
            case "!finishall":
                break;
            case "!finish":
                break;
            case "!kick":
                if (msgArray.Length > 1)
                {
                    int playerIndex = m_players.CheckIfPlayer(msgArray[1].ToLower());

                    if (playerIndex >= 0)
                    {
                        m_players.RemovePlayer(playerIndex);

                        m_irc.SendMsg(msgArray[1].ToLower() + " has been removed from the game");
                    }
                    else
                    {
                        m_irc.SendMsg(msgArray[1].ToLower() + " is not in the current game");
                    }
                }
                else
                {
                    m_irc.SendMsg(user + " invalid user");
                }
                break;
        }
    }

    private void KickPlayer()
    {

    }
}
