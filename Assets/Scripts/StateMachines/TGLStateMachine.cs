using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGLStateMachine {

	TGLState currentState;
    TGLState initialState;
	public GameObject owningGameObject;
    NoState noState;

	public TGLStateMachine(GameObject _owningGameObject) {
		currentState = new IdleState (this, "Idle");
		owningGameObject = _owningGameObject;
        noState = new NoState(this, "NoState");
	}

	public void update(float deltaTime) {
		currentState.update (deltaTime);
	}

	public void transitionTo(TGLState nextState) {
		currentState.onStateLeave (nextState);
		nextState.onStateEnter (currentState);
		currentState = nextState;
	}

	public string getCurrentStateName() {
		return currentState.name;
	}

	public void bootstrapState(TGLState state) {
		initialState = currentState = state;
        currentState.onStateEnter(noState);
	}

    public void resetMachine() {
        bootstrapState(initialState);
    }

	//public void bootstrapState(string name) {

	//}

}
