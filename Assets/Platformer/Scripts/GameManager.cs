using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public TextMeshProUGUI score;
	public TextMeshProUGUI coins;
	public TextMeshProUGUI timer;

	private int currentScore = 0;
	private int currentCoins = 0;
	private int maxTime = 300;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		timer.text = (maxTime - (int) Time.time).ToString();
    }
}
