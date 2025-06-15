using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParralaxBackground : MonoBehaviour
{
    private new GameObject camera;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float length;
    private float width;
    private float yPosition;

    private void Start()
    {
        camera = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;

        width = GetComponent<SpriteRenderer>().bounds.size.y;
        yPosition = transform.position.y;
    }

    private void Update()
    {
        float distanceMovedX = camera.transform.position.x * (1 - parallaxEffect);
        float distanceToMoveX = camera.transform.position.x * parallaxEffect;

        float distanceMovedY = camera.transform.position.y * (1 - parallaxEffect);
        float distanceToMoveY = camera.transform.position.y * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMoveX, distanceToMoveY);

        if (distanceMovedX > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        else if (distanceMovedX < xPosition - length)
        {
            xPosition = xPosition - length;
        }

        if (distanceMovedY > yPosition + width)
        {
            yPosition = yPosition + width;
        }
        else if (distanceMovedY < yPosition - width)
        {
            yPosition = yPosition - width;
        }
    }

}
