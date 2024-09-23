using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using UnityEngine;

public class TimeFetcher 
{
    private const string _timeUrl = "https://yandex.com/time/sync.json";

    public async Task<long> FetchCurrentTimeAsync()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_timeUrl))
        {
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Delay(100);
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                throw new Exception("Failed to fetch time from server");
            }

            var jsonResponse = webRequest.downloadHandler.text;
            return ParseTime(jsonResponse);
        }
    }

    private long ParseTime(string json)
    {
        var data = JsonUtility.FromJson<TimeData>(json);
        return data.time;
    }

    [Serializable]
    public class TimeData
    {
        public long time;
    }
}