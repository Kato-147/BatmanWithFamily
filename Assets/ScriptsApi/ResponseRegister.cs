using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseRegister 
{
    public ResponseRegister(int status, string notification)
    {
        this.status = status;
        this.notification = notification;
    }

    /*
* "status": 0
* "notification":"đăng nhập ko thành công"

*/
    public int status { get; set; }
    public string notification { get; set; }

}


