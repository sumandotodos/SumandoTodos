using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questionaire
{
    public int Question1;
    public int Question2;
    public int Question3;
    public int YouAre;
    public string Name;
    public string School;
    public string Address;

    public Questionaire()
    {
        Question1 = Question2 = Question3 = YouAre = 0;
        Name = School = Address = "";
    }
}

public class Scheme : MonoBehaviour
{
   
}
