using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameManager : MonoBehaviour {
    public float coins;
    public float wallet;
    static GameManager _instance = null;
    public GameObject playerPrefab;
	// Use this for initialization
	void Awake () {
        if(instance) {
            DestroyImmediate(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        coins = 0;
        wallet = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static GameManager instance {
        get { return _instance; }
        set { _instance = value; }
    }

    public IEnumerator SwitchToVR() {
        string desiredDevice = "cardboard";
        if (string.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0) {
            XRSettings.LoadDeviceByName(desiredDevice);
            yield return null;
        }
        XRSettings.enabled = true;
    }

    public IEnumerator SwitchTo2D() {
        XRSettings.LoadDeviceByName("");
            
        yield return null;
        ResetCameras();
    }

    public void ResetCameras() {
        for (int i = 0; i < Camera.allCameras.Length; i++) {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None) {
                cam.transform.localPosition = Vector3.zero;
                cam.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
