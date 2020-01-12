using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Milestone {
    public Vector3 position;
    public float delay;
}

public class Trajectory : MonoBehaviour {

    public Milestone[] milestones;
    public float speed;

    public bool loop;

    public bool autogo;

 
    Vector3 curPos;
    Vector3 nextPos;
    float segmentLength;
    Vector3 velocity;
    float t;

    int state = 0;
    const int DELAYING = 1;
    const int MOVING = 2;
    const int STOPPED = 0;
    int curMilestone = 0;
    float remaining;
    Vector3 previousPosition;
    Vector3 currentVelocity;

	// Use this for initialization
	void Start () {
        curPos = milestones[0].position;
        remaining = 0.0f;
        curMilestone = 0;
        state = STOPPED;
        if(autogo) go();
	}
	
	// Update is called once per frame
	void Update () {

        previousPosition = this.transform.localPosition;
		
        if(state == STOPPED) {

        }
        if(state == MOVING) {
            this.transform.localPosition = Vector3.Lerp(curPos, nextPos, t);
            t += (speed / segmentLength) * Time.deltaTime;
            if(t >= 1.0f) {
                remaining = milestones[curMilestone].delay;
                state = DELAYING;
            }
        }
        if(state == DELAYING) {
            remaining -= Time.deltaTime;
            if(remaining <= 0.0f) {
                curMilestone = (curMilestone + 1) % milestones.Length;
                if((curMilestone == milestones.Length-1) && !loop) {

                    state = STOPPED;

                }
                else {

                    calculateVelocity();
                    state = MOVING;

                }
            }
        }

        currentVelocity = (this.transform.localPosition - previousPosition) / Time.deltaTime;

	}

    public void go() {
        state = MOVING;
        calculateVelocity();
        curMilestone = 0;
    }

    public void goToMilestone(int n) {
        state = MOVING;
    }

    public Vector3 GetVelocity() {

        return currentVelocity;
    }

    private void calculateVelocity() {
        curPos = milestones[curMilestone].position;
        nextPos = milestones[(curMilestone + 1) % milestones.Length].position;
        segmentLength = (curPos - nextPos).magnitude;
        t = 0;
    }
}
