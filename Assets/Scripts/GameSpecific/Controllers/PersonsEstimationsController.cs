using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonsEstimationsController : MonoBehaviour {

    public GameObject smallPersons;
    public GameObject bigPersons;
    public GameObject timesPersons;

    public UIFader[] smallPerson;
    public UIFader[] bigPerson;
    public UITextFader timesLabel;

    bool _enabled = false;
    public void SetEnabled(bool en)
    {
        _enabled = en;
    }

    int CoRoutineN;
    UIFader[] CoRoutinePersons;
    IEnumerator ShowPersonsCoRoutine()
    {
        if (_enabled)
        {
            for (int i = 0; i < CoRoutinePersons.Length; ++i)
            {
                if (i < CoRoutineN) CoRoutinePersons[i].fadeToOpaque();
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    public void SetLabelEstimations(int n)
    {
        if (_enabled)
        {
            smallPersons.SetActive(false);
            bigPersons.SetActive(false);
            timesPersons.SetActive(true);
            timesLabel.GetComponent<Text>().text = "x " + n;
            timesPersons.GetComponent<UIGeneralFader>().Start();
            timesPersons.GetComponent<UIGeneralFader>().fadeToOpaque();
        }
    }

    public void SetSmallEstimations(int n)
    {
        if (_enabled)
        {
            smallPersons.SetActive(true);
            bigPersons.SetActive(false);
            timesPersons.SetActive(false);
            smallPersons.GetComponent<UIGeneralFader>().Start();
            CoRoutineN = n;
            CoRoutinePersons = smallPerson;
            StartCoroutine("ShowPersonsCoRoutine");
        }
    }

    public void SetBigEstimations(int n)
    {
        if (_enabled)
        {
            smallPersons.SetActive(false);
            bigPersons.SetActive(true);
            timesPersons.SetActive(false);
            bigPersons.GetComponent<UIGeneralFader>().Start();
            CoRoutineN = n;
            CoRoutinePersons = bigPerson;
            StartCoroutine("ShowPersonsCoRoutine");
        }
    }

    public void SetEstimations(int n)
    {
        if (_enabled)
        {
            if (n < bigPerson.Length)
            {
                SetBigEstimations(n);
            }
            else if (n < smallPerson.Length)
            {
                SetSmallEstimations(n);
            }
            else
            {
                SetLabelEstimations(n);
            }
        }

    }

    public void fadeAllToOpaque()
    {
        if (CoRoutinePersons == null) return;
        for (int i = 0; i < CoRoutinePersons.Length; ++i)
        {
            if (i < CoRoutineN) CoRoutinePersons[i].fadeToOpaque();
        }
        _enabled = true;
    }

    public void fadeAllToTransparent()
    {
        if (CoRoutinePersons == null) return;
        for (int i = 0; i < CoRoutinePersons.Length; ++i)
        {
            if (i < CoRoutineN) CoRoutinePersons[i].fadeToTransparent();
        }
        _enabled = false;
    }

}
