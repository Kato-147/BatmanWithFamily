using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ResetPassword : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField edtUser, edtPassword,edtResPass,edtOtp;
    public TMP_Text txtErorr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RessetPassword()
    {
        string user = edtUser.text;
        string pass = edtPassword.text;
        string rePass = edtResPass.text;
        int otp = int.Parse(edtOtp.text);

        if (pass != rePass)
        {
            Debug.Log("edtRePass phai trung voi edtPassword");
            txtErorr.text = "mật khẩu bạn nhập không khớp";
            return; // Ngừng thực hiện phương thức nếu điều kiện không được đáp ứng
        }

        ModelResetPassword modelResetPassword = new ModelResetPassword(user,otp,pass);
       
        StartCoroutine(resetPassword(modelResetPassword));
        resetPassword(modelResetPassword);
    }
    IEnumerator resetPassword(ModelResetPassword modelResetPassword)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(modelResetPassword);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/reset-password", "POST");
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

                txtErorr.text = "Đổi mật khẩu thành công'-'";
                edtUser.text = "";
                edtPassword.text = "";
                edtResPass.text = "";
                edtOtp.text = "";
            }
        }
        request.Dispose();
    }
}
