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
    private LevelController m_levels;

    // Use this for initialization
    void Start()
    {
        m_minigolf = this.GetComponent<MinigolfController>();
        m_players = this.GetComponent<GameJoin>();
        m_irc = this.GetComponent<TwitchIRC>();
        m_levels = this.GetComponent<LevelController>();
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
                //Gets all balls
                GameObject[] balls = m_minigolf.Balls;
                List<int> activePlayers = m_players.GetActivePlayers();

                for (int i = 0; i < activePlayers.Count; i++)
                {
                    balls[activePlayers[i]].GetComponent<Ball>().ResetBall();
                }

                m_irc.SendMsg("All players have been reset");
                break;
            case "!reset":

                break;
            case "!nextcourse":
                break;
            case "!finishall":
                //Gets all active playerds
                List<int> activePlayer = m_players.GetActivePlayers();

                for (int i = 0; i < activePlayer.Count; i++)
                {
                    //Force finishes all active players
                    m_levels.FinishBall(activePlayer[i]);
                }
                break;
            case "!finish":
                //Prevents single word sentances from breaking
                if (msgArray.Length > 1)
                {
                    //Get the player
                    int playerIndex = m_players.CheckIfPlayer(msgArray[1].ToLower());

                    if (playerIndex >= 0)
                    {
                        //Finish the players ball
                        m_levels.FinishBall(playerIndex);

                        m_irc.SendMsg(msgArray[1].ToLower() + " has forcefully finished");
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
            case "!kick":
                //Prevents single word sentances from breaking
                if (msgArray.Length > 1)
                {
                    //Get the players indnex
                    int playerIndex = m_players.CheckIfPlayer(msgArray[1].ToLower());

                    if (playerIndex >= 0)
                    {
                        //Removes the player
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
