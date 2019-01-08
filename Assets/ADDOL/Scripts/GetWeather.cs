using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using Valve.Newtonsoft.Json.Linq;

public class GetWeather : MonoBehaviour {

    //TODO aller chercher les infos dans un fichier json
    private string request = "http://api.openweathermap.org/data/2.5/weather?q=";
    private string city = "";
    private string unit = "&units=";
    private string apiKey = "&appid=";

    public Text temperature;
    public Text weather;

    void Start()
    {
        getRequestOptions();
        StartCoroutine(GetText());
    }

    public void getRequestOptions()
    {
        string filepath = "Assets/StreamingAssets/weatherConfig.json";
        using (StreamReader r = new StreamReader(filepath))
        {
            var json = r.ReadToEnd();
            var jobj = JObject.Parse(json);
            foreach (var item in jobj.Properties())
            {
                switch (item.Name){
                    case "city":
                        city += item.Value.ToString();
                        break;
                    case "units":
                        unit += item.Value.ToString();
                        break;
                    case "apiKey":
                        apiKey += item.Value.ToString();
                        break;
                }
            }
        }
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(request + city + unit + apiKey))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(city + " " + unit + " " + apiKey);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    //Debug.Log(jsonResult);

                    RootObject main = JsonUtility.FromJson<RootObject>(jsonResult);
                    Debug.Log(main.name);
                    Debug.Log(main.main.temp);
                    Debug.Log(main.weather[0].main);
                    temperature.text = main.main.temp.ToString();
                    weather.text = main.weather[0].main;
                }
            }
        }
    }
}
