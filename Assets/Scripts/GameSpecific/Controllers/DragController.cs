using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public ImagineController imagineController;
    public Transform SurfaceTransform;
    TweenableSoftFloat SurfaceRotation;
    public float factor = -0.20f;
    public float MoveThreshold = 3.0f;
    public float ReleaseThreshold = 300.0f;
    public float resolutionIndependentFactor = 280.0f;
    public UIFader redDrag;
    public UIFader greenDrag;
    bool isDragging = false;
    Vector3 TouchCoords;
    Vector3 LastCoords;
    Vector3 currentCoords;
    // Start is called before the first frame update
    void Start()
    {
        SurfaceRotation = new TweenableSoftFloat();
        SurfaceRotation.setEaseType(EaseType.cubicOut);
        SurfaceRotation.setSpeed(10.0f);
        redDrag.Start();
        redDrag.setOpacity(0.0f);
        greenDrag.Start();
        greenDrag.setOpacity(0.0f);

    }

    public void StartDrag()
    {
        if (!interactable) return;
        isDragging = true;
        currentCoords = TouchCoords = normalizedMouseInput();
    }

    private float clampMap(float inValue, float inMin, float inMax, float outMin, float outMax)
    {
        if (inValue < inMin) return outMin;
        if (inValue > inMax) return outMax;
        float param = (inValue - inMin) / (inMax - inMin);
        return Mathf.Lerp(outMin, outMax, param);
    }

    public void EndDrag()
    {
        if (!interactable) return;
        isDragging = false;
        SurfaceRotation.setValue(0.0f);

        float speed = (normalizedMouseInput() - LastCoords).magnitude
         / Time.deltaTime;
        float direction = (normalizedMouseInput() - LastCoords).y
         < 0.0f ? 1.0f : -1.0f;
        float yDist = (currentCoords - TouchCoords).y;
        SurfaceRotation.setValueImmediate(yDist * factor);
        SurfaceTransform.transform.localRotation = 
        Quaternion.Euler(0, 0, yDist * factor);
       // Debug.Log("" + yDist * factor);

        if (Mathf.Abs(yDist * factor) > 3.5f || speed > ReleaseThreshold)
        {
            SurfaceRotation.setValue(20.0f * direction);
            imagineController.SubmitVote(currentVote);
            IntuitionPointsController.AddScoreAndSave(5);
            imagineController.NextImagine();
            redDrag.SetSpeed (1.0f);
            greenDrag.SetSpeed (1.0f);
        }
        else
        {
            SurfaceRotation.setValue(0.0f);
            redDrag.SetSpeed (4.0f);
            greenDrag.SetSpeed (4.0f);
        }

        redDrag.fadeToTransparent();
        greenDrag.fadeToTransparent();


    }

    public Vector3 normalizedMouseInput()
    {
        float dimension = Screen.width;
        if (Screen.height > Screen.width) dimension = Screen.height;
        return (Input.mousePosition * resolutionIndependentFactor) / dimension;
    }

    int currentVote;

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            LastCoords = currentCoords;
            currentCoords = normalizedMouseInput();
            float yDist = (currentCoords - TouchCoords).y;
            if(Mathf.Abs(yDist) > MoveThreshold)
            {
                imagineController.CancelSwitch();
            }
            float angle = yDist * factor;
            if(angle > 0.0f)
            {
                float redOp = clampMap(angle, 0.5f, 8.0f, 0, 1);
                redDrag.setOpacity(redOp);
               
                currentVote = -1;
            }
            else
            {
                float greenOp = clampMap(angle, -8.0f, -0.5f, 1, 0);
                greenDrag.setOpacity(greenOp);

                currentVote = 1;
            }
            SurfaceRotation.setValueImmediate(angle);
            SurfaceTransform.transform.localRotation = Quaternion.Euler(0, 0, angle);
            return;
        }

        if (SurfaceRotation.update())
        {
            SurfaceTransform.transform.localRotation = Quaternion.Euler(0, 0, SurfaceRotation.getValue());
        }
        else
        {
            SurfaceTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Reset()
    {
        SurfaceTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public bool interactable = false;

    public void SetInteractEnabled(bool en)
    {
        interactable = en;
    }
}
