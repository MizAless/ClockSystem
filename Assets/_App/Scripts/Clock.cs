using System;
using System.Collections;
using UnityEngine;

public class Clock
{
    private int _currentHour;
    private int _currentMinute;
    private int _currentSecond;

    public int CurrentHour
    {
        get { return _currentHour; }
        private set
        {
            _currentHour = value;

            if (_currentHour >= 24)
                CurrentHour %= 24;
        }
    }
    public int CurrentMinute
    {
        get { return _currentMinute; }
        private set
        {
            _currentMinute = value;

            if (_currentMinute >= 60)
            {
                CurrentHour += CurrentMinute / 60;
                CurrentMinute %= 60;
            }
        }
    }

    public int CurrentSecond
    {
        get { return _currentSecond; }
        private set
        {
            _currentSecond = value;

            if (_currentSecond >= 60)
            {
                CurrentMinute += CurrentSecond / 60;
                CurrentSecond %= 60;
            }
        }
    }

    public event Action Changed;

    public void SetClock(long utcTime)
    {
        var localDateTime = DateTimeOffset.FromUnixTimeMilliseconds(utcTime).ToLocalTime();
        CurrentHour = localDateTime.Hour;
        CurrentMinute = localDateTime.Minute;
        CurrentSecond = localDateTime.Second;

        Changed?.Invoke();
    }

    public void Start()
    {
        CoroutineProvider.Instance.StartCoroutine(UpdatingClock());
    }

    private IEnumerator UpdatingClock()
    {
        float second = 1f;

        var wait = new WaitForSeconds(second);

        yield return wait;

        while (true)
        {
            CurrentSecond++;
            Changed?.Invoke();

            yield return wait;
        }
    }
}
