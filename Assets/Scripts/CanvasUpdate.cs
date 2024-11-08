using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    public TMP_Text Score;
    public TMP_Text Kills;
    public float health = 100;
    public GameObject settingsPanel;

    public static CanvasUpdate instance;
    public AudioSource explode_sound;
    public Image healthbar;
    public Image killbar;
    public Image Z_filler;
    public Image OnVol, OffVol;
    public Image play, pause;

    public Image WinBoard;
    public bool isWin = false;
    public Image LossBoard;
    public bool isLoss = false;

    float z_fill = 5;
    float z_curr = 0;

    public bool isZFull = false;
    bool isUpdate = false;
    bool VolStatus = true;
    public int kills = 0;
    public int targetkills = 10;

    public static bool gameIsPaused;

    public GameObject futureMsg;


    public GameObject shield;

    public AudioSource GameMusic;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Score.text = "Health : " + health;
        Kills.text = "Kills :  " + kills + "/" + targetkills;
    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            settingsPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }

        GameQuit();

        ShieldActivate();

        VolControl();

        PlayNext();

        Retry();

        GamePausePlay();


        if (isUpdate)
        {
            shield.SetActive(true);
            z_curr -= (Time.deltaTime * 0.5f);
            Z_filler.fillAmount = z_curr / 5.0f;
            if(z_curr <= 0)
            {
                z_curr = 0;
                isUpdate = false;
                shield.SetActive(false);
            }
        }

    }

    public void PlayMethod()
    {
        Cursor.lockState = CursorLockMode.Locked;
        settingsPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void SettingsMethod()
    {
        Debug.Log("Settings btn clicked!");
    }

    public void QuitMethod()
    {
        Debug.Log("Quit btn clicked!");
    }

    private void GameQuit()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }


    private void ShieldActivate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isZFull)
            {
                isZFull = false;
                isUpdate = true;
            }
        }
    }

    private void VolControl()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (OnVol.IsActive())
            {
                OnVol.gameObject.SetActive(false);
                OffVol.gameObject.SetActive(true);
                GameMusic.Pause();
                VolStatus = false;
            }
            else
            {
                OnVol.gameObject.SetActive(true);
                OffVol.gameObject.SetActive(false);
                GameMusic.Play();
                VolStatus = true;
            }
        }
    }

    private void PlayNext()
    {
        // Core Loop mechanic
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isWin)
            {
                targetkills += 10;
                kills = 0;
                isWin = false;
                WinBoard.gameObject.SetActive(false);
                killbar.fillAmount = 0;
                healthbar.fillAmount = 1f;
                health = 100f;
                Kills.text = "Kills :  " + kills + "/" + targetkills;
                futureMsg.SetActive(false);
            }
        }
    }

    private void Retry()
    {
        // Loss mechanic
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isLoss)
            {
                isLoss = false;
                LossBoard.gameObject.SetActive(false);
                kills = 0;
                targetkills = 20;
                killbar.fillAmount = 0;
                healthbar.fillAmount = 1f;
                health = 100f;
                Kills.text = "Kills :  " + kills + "/" + targetkills;
            }
        }
    }


    private void GamePausePlay()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameIsPaused = !gameIsPaused;
            IsPauseGame();
        }
    }


    void IsPauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void takeDamage(float dam)
    {
        if (health != 0)
        {
            if (!isUpdate)
            {
                health -= dam;
                Score.text = "Health : " + health;
                healthbar.fillAmount = health / 100f;
            }

            if (health == 0)
            {
                isLoss = true;
                LossBoard.gameObject.SetActive(true);
            }
            
        }
        
    }

    public void SuccessKill()
    {
        if (kills < targetkills)
        {
            kills++;
            Kills.text = "Kills :  " + kills + "/" + targetkills;
            killbar.fillAmount = (float) kills / targetkills;

            if (z_curr < 5)
            {
                if (!isUpdate)
                {
                    z_curr++;
                    Z_filler.fillAmount = z_curr / z_fill;
                }
                
            }

            if (z_curr == 5)
            {
                isZFull = true;
            }

            if (kills == targetkills)
            {
                WinBoard.gameObject.SetActive(true);
                isWin = true;
                futureMsg.SetActive(true);
            }
        }

    }

    public void explode_audio()
    {
        if (VolStatus)
        {
            explode_sound.Play();
        }
        
    }
}
