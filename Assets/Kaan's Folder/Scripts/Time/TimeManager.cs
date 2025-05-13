using MountAndBlade;
using Oms;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static Action OnYearChange;
    public static Action OnMonthChange;
    public static Action OnDayChange;
    public static Action OnMinuteChange; // DK de�i�ti�inde triggerlan�yor
    public static Action OnHourChange;   // SAAT de�i�ti�inde triggerlan�yor
    public PlayerController PlayerController;
   

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }
    public static int MonthIndex { get; private set; }
    public static int Year { get; private set; }
    
    public static float timer;

    [SerializeField]
    private float desiredTimeScale = 3.0f; // Shift ile oyun ne kadar hizlandırılacak
    [SerializeField]
    private float minuteToRealTime = 0.5f;  // Ger�ekte 1 saniye oyunda 0.5 saniyeye e�it
    [SerializeField]
    private GameObject Player;

    private bool isTimeActive;

    public TextMeshProUGUI pauseText;
    private NavMeshAgent playerAgent;

    public float checkDelay;
    public float speedTreshold;

    private bool isPaused;
    private Coroutine checkCoroutine;

    private void Awake() => TimeModifer();
    void Start()
    {
        LoadTime();
        playerAgent = Player.GetComponent<NavMeshAgent>();
        PlayerController = Player.GetComponent<PlayerController>();
        timer = minuteToRealTime;
    }

    void Update()
    {
        InGameTime();
        TimeSpeedHandler();
        if(isTimeActive) TimeHandler();

        if (isPaused)
        {
            if (PlayerController.thereIsPath)
            {
                ResumeGame();


                if (checkCoroutine != null)
                    StopCoroutine(checkCoroutine);

                checkCoroutine = StartCoroutine(CheckVelocityAfterDelay());
            }
        }
        else {

            if (playerAgent.velocity.magnitude <= speedTreshold)
            {

                PauseGame();
            }
        
        }

    }
   
    IEnumerator CheckVelocityAfterDelay()
    {
        yield return new WaitForSeconds(checkDelay);

        if (playerAgent.velocity.magnitude <= speedTreshold)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
    private void InGameTime()
    {
        if(Player != null)
        {
            if(playerAgent != null)
            {
                bool isSpacePressed = Input.GetKey(KeyCode.Space);
                //bool isAgentMoving = playerAgent.velocity.sqrMagnitude > 0.05f;



               //isTimeActive = isSpacePressed || isAgentMoving;
                isTimeActive = isSpacePressed || !isPaused;
                pauseText.text = isTimeActive ? " " : "PAUSED";

                
            }
        }
    }
    
    private void TimeHandler()
    {
        if (isTimeActive)
        
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
    private void TimeSpeedHandler()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Time.timeScale = desiredTimeScale;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            Time.timeScale = 1.0f;
    }

    public void TimeScaleForInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) && Time.timeScale > 0.1)
        {
            Time.timeScale = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.I) && Time.timeScale < 0.1)
        {
            Time.timeScale = 1.0f;
        }
    }
    private void TimeModifer()
    {
        isTimeActive = true;
        Year = 1453;
        Day = 29;
        MonthIndex = 11;
        Minute = 50;
        Hour = 17;
        SaveTime();
    }
    private void OnApplicationQuit() => SaveTime();
    private void LoadTime()
    {
        Minute = PlayerPrefs.GetInt(nameof(Minute));
        Hour = PlayerPrefs.GetInt(nameof(Hour));
        Day = PlayerPrefs.GetInt(nameof(Day));
        MonthIndex = PlayerPrefs.GetInt(nameof(MonthIndex));
        Year = PlayerPrefs.GetInt(nameof(Year));
    }
    private void SaveTime()
    {
        PlayerPrefs.SetInt(nameof(Minute), Minute);
        PlayerPrefs.SetInt(nameof(Hour), Hour);
        PlayerPrefs.SetInt(nameof(Day), Day);
        PlayerPrefs.SetInt(nameof(MonthIndex), MonthIndex);
        PlayerPrefs.SetInt(nameof(Year), Year);
    }
    
}
