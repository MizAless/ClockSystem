using System;
using UnityEngine;
using System.Threading.Tasks;

public class Root : MonoBehaviour
{
    [SerializeField] private ClockView _clockView;

    private Clock _clock;
    private TimeFetcher _timeFetcher;

    private void Awake()
    {
        _clock = new Clock();
        _timeFetcher = new TimeFetcher();
        _clockView.Init(_clock);
    }

    private async void Start()
    {
        await StartTimeSynchronization();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 100f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private async Task StartTimeSynchronization()
    {
        while (true)
        {
            await InitializeClock();
            await Task.Delay(TimeSpan.FromHours(1));
        }
    }

    private async Task InitializeClock()
    {
        try
        {
            long utcTime = await _timeFetcher.FetchCurrentTimeAsync();
            _clock.SetClock(utcTime);
            _clock.Start();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}
