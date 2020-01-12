using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGLStateTransition {
	public TGLState nextState;
	public System.Func<TGLState, bool> check;
	public TGLStateTransition(TGLState _nextState, System.Func<TGLState, bool> _check) {
		nextState = _nextState;
		check = _check;
	}
}

public abstract class TGLState {

	List<TGLState> substates;
	List<TGLStateTransition> transitions;
	public TGLStateMachine owningMachine;
	public string name;

	public TGLState(TGLStateMachine _owningMachine, string _name) {
		substates = new List<TGLState> ();
		transitions = new List<TGLStateTransition> ();
		owningMachine = _owningMachine;
		name = _name;
	}

	public void addTransition(TGLState nextState, System.Func<TGLState, bool> _check) {
		transitions.Add (new TGLStateTransition (nextState, _check));
	}

	public abstract void onStateEnter (TGLState prevState);
	public abstract void onStateLeave (TGLState nextState);
	public abstract void updateTimeSlice (float deltaTime);
	public void update(float deltaTime) {
		updateTimeSlice (deltaTime);
		foreach (TGLStateTransition transition in transitions) {
			if (transition.check (this)) {
				owningMachine.transitionTo (transition.nextState);
			}
		}
	}

}

public class IdleState : TGLState {
	public IdleState(TGLStateMachine _owningMachine, string _name) : base(_owningMachine, _name) {

	}
	public override void onStateEnter (TGLState prevState) {
		
	}
	public override void onStateLeave(TGLState nextState) {
	
	}
	public override void updateTimeSlice(float deltaTime) {

	}
}

public class NoState : TGLState {
    public NoState(TGLStateMachine _owningMachine, string _name) : base(_owningMachine, _name)
    {

    }
    public override void onStateEnter(TGLState prevState)
    {

    }
    public override void onStateLeave(TGLState nextState)
    {

    }
    public override void updateTimeSlice(float deltaTime)
    {

    }
}