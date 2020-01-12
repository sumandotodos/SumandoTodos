﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGeneralFader : FGProgram {

	UIFader[] imageFaders;
	UITextFader[] textFaders;

    bool refreshed = false;

    public int CountCheck;

	// Use this for initialization
	bool started = false;
	public void Start () {
		if (started)
			return;
		started = true;
        refreshChildren();
	}
	
    public void refreshChildren()
    {
        if (refreshed) return;
        refreshed = true;
        imageFaders = this.GetComponentsInChildren<UIFader>();
        textFaders = this.GetComponentsInChildren<UITextFader>();
        foreach (UIFader f in imageFaders) f.Start();
        foreach (UITextFader f in textFaders) f.Start();
        CountCheck = imageFaders.Length + textFaders.Length;
    }

    public void SetOpacity(float op)
    {
        Start();
        refreshChildren();
        foreach (UIFader f in imageFaders)
        {
            f.Start();
            f.setOpacity(op);
        }
        foreach (UITextFader f in textFaders)
        {
            f.Start();
            f.setOpacity(op);
        }
    }

    public void SetMaxOpacity(float Max)
    {
        Start();
        refreshChildren();
        foreach (UIFader f in imageFaders)
        {
            f.Start();
            f.maxOpacity = Max;
        }
        foreach (UITextFader f in textFaders)
        {
            f.Start();
            f.maxOpacity = Max;
        }
    }

    public void fadeToOpaque() {
		Start ();
        refreshChildren();
		foreach (UIFader f in imageFaders) {
			f.Start ();
			f.fadeToOpaque ();
		}
		foreach (UITextFader f in textFaders) {
			f.Start ();
			f.fadeToOpaque ();
		}
	}

	public void fadeToTransparent() {
		Start ();
        refreshChildren();
        foreach (UIFader f in imageFaders) {
			f.Start ();
			f.fadeToTransparent ();
		}
		foreach (UITextFader f in textFaders) {
			f.Start ();
			f.fadeToTransparent ();
		}
	}

	public void fadeToTransparentImmediately() {
		Start ();
        refreshChildren();
        foreach (UIFader f in imageFaders) {
			f.Start ();
			f.fadeToTransparentImmediately ();
		}
		foreach (UITextFader f in textFaders) {
			f.Start ();
			f.fadeToTransparentImmediately ();
		}
	}

	public void fadeToOpaqueImmediately() {
		Start ();
        refreshChildren();
        foreach (UIFader f in imageFaders) {
			f.Start ();
			f.fadeToOpaqueImmediately ();
		}
		foreach (UITextFader f in textFaders) {
			f.Start ();
			f.fadeToOpaqueImmediately ();
		}
	}
}
