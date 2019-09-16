using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DigitalRuby.Tween;

public class NotificationManager : ManagerEvents
{
    //@TODO add flexibility for different kinds of notifications;
    
    public GameObject notificationUI;
    public TextMeshProUGUI notificationText;
    Color transparentColor = new Color(255F, 255F, 255F, 0F);
    Color opaqueColor = new Color(255F, 255F, 255F, 1F);
    private Image image;
    private AudioSource source;
    public AudioClip notificationSound;


    private void Start()
    {
        image = notificationUI.GetComponent<Image>();
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
    }

    private void OnDisable()
    {
        ManagerEvents.DetectiveRemarkEnded -= OnDetectiveRemarkEnd;
    }

    private void OnDetectiveRemarkEnd(Clue clue)
    {
        if (clue != null && !clue.hasBeenFound && clue.isJournalEntry)
        {
            StartCoroutine(DisplayNewClueNotification());
        }
    }

    IEnumerator DisplayNewClueNotification()
    {
        notificationText.text = "New clue found.";
        Tween(notificationUI,true);
        source.PlayOneShot(notificationSound);
        yield return new WaitForSeconds(3f);
        KillNotification();
    }

    private void KillNotification()
    {
        Tween(notificationUI,false);
    }
    
    private void Tween(GameObject go,bool isAppearing)
    {
        Color startColor;
        Color endColor;
        if (isAppearing)
        {
            startColor = transparentColor;
            endColor = opaqueColor;
        }
        else
        {
            startColor = opaqueColor;
            endColor = transparentColor;
        }
        go.Tween(null,startColor,endColor,0.7f, TweenScaleFunctions.CubicEaseInOut, (t) =>
        {
            // progress
            image.color = t.CurrentValue;
        }, (t) =>
        {
            // completion
        });
        
    }
}

