using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurnLeft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject objects;
    public float rotationMultiplierLeft = 1f;
    private bool down = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        down = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(down)
        {
            rotateLeft();
        }
    }

    public void rotateLeft()
    {
        objects.transform.Rotate(0.0f, 0.0f, rotationMultiplierLeft, Space.World);
    }


}
