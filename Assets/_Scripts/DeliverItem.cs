using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts
{
    public class DeliverItem : MonoBehaviour
    {
        public GameObject gcObj;
        public GameController gc;
        public Text wrong;
        public Text win;
        private IEnumerator coroutine;

        private void Start()
        {
            gc = gcObj.GetComponent<GameController>();
            wrong.enabled = false;
            win.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
            if (other.gameObject.tag == "Special")
            {
                gc.timerIsRunning = false;
                Debug.Log("Item delivered");
                other.transform.SetParent(null);
                Destroy(other.gameObject);
                StartCoroutine(GameWin());
                
            }
            if (other.gameObject.tag == "Item") {
                StartCoroutine(ShowMessage());
                Destroy(other.gameObject);
            }
        }

        IEnumerator <WaitForSeconds> ShowMessage () {
            wrong.text = "Wrong Crate!";
            wrong.enabled = true;
            yield return new WaitForSeconds(2);
            wrong.enabled = false;
        }
        
        IEnumerator <WaitForSeconds> GameWin ()
        {
                             
            win.text = "Continue in 5 seconds";
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("main");
        }
    }
}