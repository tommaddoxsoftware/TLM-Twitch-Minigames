using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigolfBotReplys
{
    private TwitchIRC m_irc;

    public MinigolfBotReplys(TwitchIRC irc)
    {
        m_irc = irc;
    }

    private string ResponseStart(string user)
    {
        //Customise responses start
        return user + " ";
    }

    public void Hit(string user)
    {
        string response = ResponseStart(user);
        
        //Response text
        response += "hit the ball";

        m_irc.SendMsg(response);
    }

    public void InvalidHit(string user)
    {
        string response = ResponseStart(user);

        //Response text
        response += "cannot hit the ball whilst it is moving";

        m_irc.SendMsg(response);
    }

    public void Angle(string user, int angle)
    {
        string response = ResponseStart(user);

        //Response text
        response += "new angle set to: " + angle;

        m_irc.SendMsg(response);
    }

    public void OverMaxAngle(string user)
    {
        string response = ResponseStart(user);

        //Response text
        response += "value over max, setting to: 360";

        m_irc.SendMsg(response);
    }

    public void UnderMinAngle(string user)
    {
        string response = ResponseStart(user);

        //Response text
        response += "value under min, setting to: 0";

        m_irc.SendMsg(response);
    }

    public void Adjust(string user, int adjVal, int newAng)
    {
        string response = ResponseStart(user);

        //Response text
        response += "adjusting angle by: " + adjVal + ". New angle: " + newAng;

        m_irc.SendMsg(response);
    }

    public void Power(string user, float power)
    {
        string response = ResponseStart(user);

        //Response text
        response += "new power set to: " + power;

        m_irc.SendMsg(response);
    } 

    public void OverMaxPower(string user, float maxPower)
    {
        string response = ResponseStart(user);

        //Response text
        response += "value over max, setting to: " + maxPower;

        m_irc.SendMsg(response);
    }

    public void UnderMinPower(string user, float minPower)
    {
        string response = ResponseStart(user);

        //Response text
        response += "value under min, setting to: " + minPower;

        m_irc.SendMsg(response);
    }

    public void Reset(string user)
    {
        string response = ResponseStart(user);

        response += "ball has been reset";

        m_irc.SendMsg(response);
    }

    public void GolfCommands()
    {
        //Message for each line because no new line exists
        m_irc.SendMsg("!hit: propells the ball forward");
        m_irc.SendMsg("!angle or !an: sets the angle of the ball relative to up");
        m_irc.SendMsg("!adjust or !ad: adjusts the angle of the ball relative to its current angle");
        m_irc.SendMsg("!power or !pow: sets the power of the ball");
        m_irc.SendMsg("!reset: resets you ball back to the beginning of the course");
    }
}
