using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI pointsTxt;
    public AudioClip coinSnd;
    public AudioSource audioSource;
    private int points;
    public static UIManager Instance;
    private void Awake() 
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start() 
    {
        points = 0;
        pointsTxt.text = points.ToString();
    }
    private void OnEnable() 
    {
        Crystal.OnCoinPick += GainPoints;
        PlayerController.OnDamage += LosePoints;
    }
    private void OnDisable() 
    {
        Crystal.OnCoinPick -= GainPoints;
        PlayerController.OnDamage -= LosePoints;
    }
    private void GainPoints()
    {
        audioSource.PlayOneShot(coinSnd);
        points += 50;
        pointsTxt.text = points.ToString();
    }
    private void LosePoints()
    {
        points -= 50;
        if (points < 0) points = 0;
        pointsTxt.text = points.ToString();
    }
    public int GetPoints()
    {
        return points;
    }
}
