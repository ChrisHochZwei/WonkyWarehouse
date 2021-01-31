using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {

        public float totalPoints = 10000;
        public int displayPoints;
        public Text victoryPoints;
        public Text points;
        public bool timerIsRunning = false;
        private bool printedPoints = false;

        private void Start()
        {
            // Starts the timer automatically
            timerIsRunning = true;
            victoryPoints.enabled = false;
        }

        void Update()
        {
            if (timerIsRunning)
            {
                if (totalPoints > 0)
                {
                    totalPoints -= Time.deltaTime;
                    displayPoints = (int) totalPoints ;
                    
                    points.text = "Points: " + displayPoints;
                }
                else
                {
                    Debug.Log("Time has run out!");
                    totalPoints = 0;
                    timerIsRunning = false;
                }
            }
            else if (!printedPoints)
            {
                Mathf.Round(totalPoints);
                
                victoryPoints.text= "You reached " + displayPoints + " Points!";
                victoryPoints.enabled = true;
                points.enabled = false;
            }
        }
    }
}
