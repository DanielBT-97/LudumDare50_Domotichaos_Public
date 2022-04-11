using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutController : MonoBehaviour {
    public void FadeEnded() {
        GameManager.Instance.StartGame();
    }
}
