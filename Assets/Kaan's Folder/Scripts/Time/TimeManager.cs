using Oms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static Action OnYearChange;
    public static Action OnMonthChange;
    public static Action OnDayChange;
    public static Action OnMinuteChange; // DK de�i�ti�inde triggerlan�yor
    public static Action OnHourChange;   // SAAT de�i�ti�inde triggerlan�yor

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }
    public static int MonthIndex { get; private set; }
    public static int Year { get; private set; }

    [SerializeField]
    private float minuteToRealTime = 0.5f;  // Ger�ekte 1 saniye oyunda 0.5 saniyeye e�it
    public static float timer;

    

    // Saat 10:00 ' a ayarland�
    void Start()
    {
        Year = 1212;
        Day = 29;
        MonthIndex = 11;
        Minute = 50;
        Hour = 17;
        timer = minuteToRealTime;

    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Minute++;
            OnMinuteChange?.Invoke(); // Dakika değiştiğinde tetiklenir

            if (Minute >= 60)
            {
                Hour++;
                Minute = 0;
                OnHourChange?.Invoke();

                if (Hour >= 24)
                {
                    Day++;
                    Hour = 0;
                    Minute = 0;
                    OnDayChange?.Invoke();

                    // Her ay 30 gün olarak kabul ediliyor
                    if (Day >= 30)
                    {
                        MonthIndex++;
                        Day = 0;
                        Hour = 0;
                        OnMonthChange?.Invoke();

                        // Eğer 12. aya ulaştıysak (MonthIndex >= 12), yılı artır ve ayı sıfırla
                        if (MonthIndex == 12)
                        {
                            Year++;
                            Day = 0;
                            Hour = 0;
                            MonthIndex = 0;  // Ay sıfırlanır
                            OnYearChange?.Invoke();
                        }
                    }
                }
            }

            timer = minuteToRealTime;
        }
    }

}
