using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {
    public float changeSpeed;
    public Material mat;
    public Color color = Color.red;
	// Use this for initialization
	void Start () {
        if(changeSpeed <= 0) {
            changeSpeed = 1f;
        }
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(RedEyes(color));
	}

    public IEnumerator RedEyes(Color c) {
        float t = 0;
        while(t < 1) {
            t += Time.time * changeSpeed;
            mat.color = Color.Lerp(mat.color, c, t);
        }
        yield return null;
    }
}
