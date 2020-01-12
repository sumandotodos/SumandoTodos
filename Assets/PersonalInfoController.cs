using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalInfoController : FGProgram
{
    public UIGeneralFader StudentToggle;
    public UIGeneralFader StudentFamilyToggle;
    public UIGeneralFader TeacherToggle;
    public UIGeneralFader NameEntry;
    public UIGeneralFader SchoolEntry;
    public UIGeneralFader AddressEntry;
    public UITMPFader YouAreLabelFader;

    void Start()
    {
        Initialize();

        execute(YouAreLabelFader, "fadeToOpaque");
        delay(0.5f);
        execute(TeacherToggle, "fadeToOpaque");
        delay(0.15f);
        execute(NameEntry, "fadeToOpaque");
        delay(0.15f);
        execute(SchoolEntry, "fadeToOpaque");
        delay(0.15f);
        execute(AddressEntry, "fadeToOpaque");
        delay(0.05f);
        execute(StudentFamilyToggle, "fadeToOpaque");
        delay(0.5f);
        execute(StudentToggle, "fadeToOpaque");
    }

    public void Initialize()
    {
        Debug.Log("PersonalInfoController::Initialize!!");
        YouAreLabelFader.Start();
        YouAreLabelFader.fadeToTransparentImmediately();
        StudentToggle.Start();
        StudentFamilyToggle.Start();
        TeacherToggle.Start();
        NameEntry.Start();
        SchoolEntry.Start();
        AddressEntry.Start();
        DisableTeacherEntries();
        StudentToggle.fadeToTransparentImmediately();
        TeacherToggle.fadeToTransparentImmediately();
        StudentFamilyToggle.fadeToTransparentImmediately();
        NameEntry.fadeToTransparentImmediately();
        AddressEntry.fadeToTransparentImmediately();
        SchoolEntry.fadeToTransparentImmediately();
        StudentToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        StudentFamilyToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        TeacherToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);

    }

    public void EnableTeacherEntries()
    {
        NameEntry.SetOpacity(1.0f);
        AddressEntry.SetOpacity(1.0f);
        SchoolEntry.SetOpacity(1.0f);
    }

    public void DisableTeacherEntries()
    {
        NameEntry.SetMaxOpacity(0.25f);
        AddressEntry.SetMaxOpacity(0.25f);
        SchoolEntry.SetMaxOpacity(0.25f);
    }

    public void ToggleStudent(bool newOn)
    {
        if(newOn == false)
        {
            StudentToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            return;
        }

        TeacherToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        StudentFamilyToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        UpdateTeacherEntries();
    }

    public void ToggleStudentFamily(bool newOn)
    {

        if (newOn == false)
        {
            StudentFamilyToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            return;
        }
       
        StudentToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        TeacherToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        UpdateTeacherEntries();
    }

    public void UpdateTeacherEntries()
    {
        if (TeacherToggle.GetComponent<Toggle>().isOn)
        {
            EnableTeacherEntries();
        }
        else
        {
            DisableTeacherEntries();
        }
    }

    public void ToggleTeacher(bool newOn)
    {
        if (newOn == false)

        {
            TeacherToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            return;
        }

        StudentToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        StudentFamilyToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        UpdateTeacherEntries();
    }

    public void DismissAll()
    {

        NameEntry.fadeToTransparent();
        SchoolEntry.fadeToTransparent();
        AddressEntry.fadeToTransparent();
        TeacherToggle.fadeToTransparent();
        StudentToggle.fadeToTransparent();
        YouAreLabelFader.fadeToTransparent();
        StudentFamilyToggle.fadeToTransparent();

    }
}
