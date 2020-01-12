using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGLStateVariable<T> {

	private T data;
	private T onStateEnterDefaultValue;
	bool onStateEnterDefaultValueSet;
	private TGLState owningState;

	public TGLStateVariable(T initialValue, TGLState _owningState) {
		data = initialValue;
		owningState = _owningState;
		onStateEnterDefaultValueSet = false;
	}

	public void set(T newData) {
		data = newData;
	}

	public T get() {
		return data;
	}

	public void setOnStateEnterDefaultValue(T value) {
		onStateEnterDefaultValue = value;
		onStateEnterDefaultValueSet = true;
	}

	public void onStateEnter() {
		if (onStateEnterDefaultValueSet) {
			data = onStateEnterDefaultValue;
		}
	}
}
