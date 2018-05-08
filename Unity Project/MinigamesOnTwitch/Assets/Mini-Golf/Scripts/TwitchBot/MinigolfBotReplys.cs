using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigolfBotReplys
{
    private int m_maxPower;

    private StringSplitter m_splitter;
    private TwitchIRC m_irc;

    public MinigolfBotReplys(TwitchIRC irc, int maxPower)
    {
        m_splitter = new StringSplitter();
        m_irc = irc;
        m_maxPower = maxPower;
    }

    public void ProcessCommand(string user, string[] messages)
    {
        //Loops through all commands submited by the user
        for (int i = 0; i < messages.Length; i++)
        {
            //Replys the the user
            string response = user + " ";

            switch (messages[i].ToLower())
            {
                case "!hit":
                    response += "hit the ball ";
                    m_irc.SendMsg(response);

                    break;
                case "!an":
                case "!angle":
                    try
                    {
                        int angVal = int.Parse(messages[i + 1]);

                        if (angVal > 360)
                            response += "value over max, setting to: 360";
                        else if (angVal < 0)
                            response += "value under min, setting to: 0";
                        else
                            response += "new angle set to: " + angVal;

                        //Increments over by 1 to compensate for the next command being a value
                        i++;
                    }
                    catch
                    {
                        response += "invalid angle value";
                    }
                    response += " ";
                    m_irc.SendMsg(response);

                    break;
                case "!ad":
                case "!adjust":
                    try
                    {
                        int adjVal = int.Parse(messages[i + 1]);

                        response += "adjusting angle by: " + adjVal;

                        //Increments over by 1 to compensate for the next command being a value
                        i++;
                    }
                    catch
                    {
                        response += "invalid adjustment value";
                    }
                    response += " ";
                    m_irc.SendMsg(response);

                    break;
                case "!pwr":
                case "!power":
                    try
                    {
                        int powVal = int.Parse(messages[i + 1]);

                        if (powVal > m_maxPower)
                            response += "value over max, setting to: " + m_maxPower;
                        else if (powVal < 1)
                            response += "value under min, setting to: 1";
                        else
                            response += "new power set to: " + powVal;
                        
                        //Increments over by 1 to compensate for the next command being a value
                        i++;
                    }
                    catch
                    {
                        response += "invalid power value";
                    }
                    response += " ";
                    m_irc.SendMsg(response);

                    break;
                case "!reset":
                    response += "reseting ball to start ";
                    m_irc.SendMsg(response);

                    break;
            }
        }

        if (messages[0].ToLower() == "!commands")
        {
            //Message for each line because no new line exists
            m_irc.SendMsg("!hit: propells the ball forward");
            m_irc.SendMsg("!angle or !an: sets the angle of the ball relative to up");
            m_irc.SendMsg("!adjust or !ad: adjusts the angle of the ball relative to its current angle");
            m_irc.SendMsg("!power or !pow: sets the power of the ball");
            m_irc.SendMsg("!reset: resets you ball back to the beginning of the course");
        }

        //Sends a the message to the Twitch chat
        
    }
}
