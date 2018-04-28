using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigolfBotReplys
{
    private StringSplitter m_splitter;
    private TwitchIRC m_irc;

    public MinigolfBotReplys(TwitchIRC irc)
    {
        m_splitter = new StringSplitter();
        m_irc = irc;
    }

    public void ProcessCommand(string user, string[] messages)
    {
        //Replys the the user
        string response = user + " ";

        //Loops through all commands submited by the user
        for (int i = 0; i < messages.Length; i++)
        {
            switch (messages[i].ToLower())
            {
                case "!hit":
                    response += "hit the ball ";
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
                    
                    break;
                case "!pwr":
                case "!power":
                    try
                    {
                        int powVal = int.Parse(messages[i + 1]);

                        if (powVal > 50)
                            response += "value over max, setting to: 50";
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

                    break;
                case "!reset":
                    response += "reseting ball to start ";
                    break;
            }
        }

        //Sends a the message to the Twitch chat
        m_irc.SendMsg(response);
    }
}
