using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelResetPassword
{
    public ModelResetPassword(int otp, string newpassword)
    {
        this.otp = otp;
        this.newpassword = newpassword;
    }

    public ModelResetPassword(string username, int otp, string newpassword)
    {
        this.username = username;
        this.otp = otp;
        this.newpassword = newpassword;
    }

    public string username { get; set; }
    public int otp { get; set; }
    public string newpassword { get; set; }

    // New constructor to match the order of parameters you are using
    public ModelResetPassword(string otp, string newpassword)
    {
        if (int.TryParse(otp, out int otpValue))
        {
            this.otp = otpValue;
        }
        else
        {
            // Handle the case where 'otp' is not a valid integer
            Debug.LogError("Invalid OTP. Please enter a valid integer.");
        }

        this.newpassword = newpassword;
    }
}

