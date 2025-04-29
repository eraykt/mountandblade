using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI stageText;

    public enum Months
    {
        January = 0,
        February = 1,
        March = 2,
        April = 3,
        May = 4,
        June = 5,
        July = 6,
        August = 7,
        September = 8,
        October = 9,
        November = 10,
        December = 11
    }


    private void OnEnable()
    {
        TimeManager.OnMinuteChange += UpdateTime;
        TimeManager.OnHourChange += UpdateTime;
        TimeManager.OnDayChange += UpdateTime;
        TimeManager.OnMonthChange += UpdateTime;
        TimeManager.OnYearChange += UpdateTime;
    }

    private void OnDisable()
    {
        TimeManager.OnMinuteChange -= UpdateTime;
        TimeManager.OnHourChange -= UpdateTime;
        TimeManager.OnDayChange -= UpdateTime;
        TimeManager.OnMonthChange -= UpdateTime;
        TimeManager.OnYearChange -= UpdateTime;
    }

    private void UpdateTime()
    {
        // Saat ve dakika formatlaması
        clockText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}";

        // Gün, ay (enum kullanarak), ve yıl formatlaması
        Months month = (Months)TimeManager.MonthIndex;  // MonthIndex'i enum'a çeviriyoruz
        dayText.text = $"{TimeManager.Day + 1} {month} {TimeManager.Year}";

        // Zaman dilimi güncellemesi
        TimeStageHandler();
    }

    private void TimeStageHandler()
    {
        switch (TimeManager.Hour)
        {
            case >= 0 and < 4:
                stageText.text = "Late Night";
                break;
            case >= 4 and < 6:
                stageText.text = "Dawn";
                break;
            case >= 6 and < 8:
                stageText.text = "Early Morning";
                break;
            case >= 8 and < 11:
                stageText.text = "Morning";
                break;
            case >= 11 and < 13:
                stageText.text = "Noon";
                break;
            case >= 13 and < 16:
                stageText.text = "Afternoon";
                break;
            case >= 16 and < 18:
                stageText.text = "Late Afternoon Sunset";
                break;
            case >= 18 and < 20:
                stageText.text = "Dusk";
                break;
            case >= 20 and < 23:
                stageText.text = "Evening";
                break;
            default:
                stageText.text = "Midnight";
                break;
        }
    }
}
