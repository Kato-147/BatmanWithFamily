using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ForgotPassword : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField edtUser;
    public TMP_Text txtErorr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendOtp()
    {
        var user = edtUser.text;
       if(user == "")
        {
            txtErorr.text = "vui lòng không để trống dữ liệu";
        }
        OtpModels otpModels = new OtpModels(user);
        StartCoroutine(SendOtp(otpModels));
        SendOtp(otpModels);
    }

    IEnumerator SendOtp(OtpModels otpModels)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(otpModels);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/send-otp", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseRegister responseRegister = JsonConvert.DeserializeObject<ResponseRegister>(jsonString);
            if (responseRegister.status == 0)
            {
                txtErorr.text = responseRegister.notification;
            }
            else
            {
                
                txtErorr.text ="send otp thành công";
                edtUser.text = "";
            }
        }
        request.Dispose();
    }
}
