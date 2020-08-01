using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class APIHttpRequest : MonoBehaviour
{

    public static void Curl(string url, HTTPMethods httpMethods, string rawJsonObject, System.Action<bool, string> callback) {
        var r = new BestHTTP.HTTPRequest(new System.Uri(url), httpMethods, (request, response) =>
        {
            if (callback == null) return;

            if (request.State == HTTPRequestStates.Finished)
            {
                callback(true, response.DataAsText);
            }
            else {
                Debug.Log(request.State);
                callback(false, "");
            }
        });
        
        if (!string.IsNullOrEmpty(rawJsonObject)) {
            r.SetHeader("Content-Type", "application/json; charset=UTF-8");
            r.RawData = System.Text.Encoding.UTF8.GetBytes(rawJsonObject);
        }

        //r.Timeout = System.TimeSpan.FromSeconds(4);
        //r.ConnectTimeout = System.TimeSpan.FromSeconds(4);

        r.Send();
    }

}
