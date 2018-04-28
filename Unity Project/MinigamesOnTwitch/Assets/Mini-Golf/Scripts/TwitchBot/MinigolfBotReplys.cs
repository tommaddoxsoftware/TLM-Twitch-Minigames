using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigolfBotReplys
{
    private StringSplitter m_splitter;
    private TwitchIRC m_irc;

    public MinigolfBotReplys(TwitchIRC irc, GameJoin players)
    {
        m_splitter = new StringSplitter();
        m_irc = irc;
    }

    public void ProcessCommand(string user, string[] messages)
    {
        //Replys the the user
        string response = user + " ";

        if (messages[0].ToLower() == "!join")
        {
            m_irc.SendMsg(response + "has joined the game");
        }

        if (messages[0].ToLower() == "!leave")
        {
            m_irc.SendMsg(response + "has left the game");
        }
        else
        {
            for (int i = 0; i < messages.Length; i++)
            {

            }
        }

        m_irc.SendMsg(response);
    }
}
