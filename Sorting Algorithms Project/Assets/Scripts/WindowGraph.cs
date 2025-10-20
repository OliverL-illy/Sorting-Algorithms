using CodeMonkey.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private void Awake()
    {
        graphContainer = transform.Find("Graph Container").GetComponent<RectTransform>();

        labelTemplateX = graphContainer.Find("Label Template X").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("Label Template Y").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("Dash Template X").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("Dash Template Y").GetComponent<RectTransform>();

        List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };
        ShowGraph(valueList);
    }

    // Method to create a circle GameObject at a specified anchored position
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        // Create a new GameObject with an Image component to represent the circle
        GameObject gameObject = new GameObject("circle", typeof(Image));

        // Set the parent of the GameObject to the graph container
        gameObject.transform.SetParent(graphContainer, false);

        // Set the sprite of the Image component to the circle sprite
        gameObject.GetComponent<Image>().sprite = circleSprite;

        // Get the RectTransform of the GameObject
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        // Set the anchored position, size, and anchors of the RectTransform, In this case, size is set to 11x11 and anchors to bottom-left
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    private void ShowGraph(List<int> valueList)
    {
        //height of the graph container (sizeDelta.y gets the height of the RectTransform)
        float graphHeight = graphContainer.sizeDelta.y;

        //size distance between each point on x axis
        float xSize = 50f;

        //maximum value on y axis (from valueList)
        float yMaximum = 100f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            // Calculate the x and y position for each data point, x is spaced by xSize + (for offset), y is scaled based on the maximum value
            float xPosition = xSize + i * xSize;
            // yposition is calculated by normalizing the value to the maximum (we do this to get a value between 0 and 1) and then multiplying by the graph height to get the actual position in the graph
            float yPosition = (valueList[i] / yMaximum) * graphHeight;

            // Create a circle at the calculated position
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

            // If there is a last circle, create a connection between the last circle and the current circle. (if statement is to skip the first point since there is no last point to connect to)
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            // Update lastCircleGameObject to the current circle for the next iteration
            lastCircleGameObject = circleGameObject;

            // Create X-axis labels
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = i.ToString();

            // Create X-axis dashes
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -20f);
        }

        // Create Y-axis labels
        int separatorCount = 10; // Number of separators on the Y-axis
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            // Normalised index (0-1) of i based on seperatorCount. Value of i proportinal to separatorCount
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            // calculate the actual value for the label based on the normalized value and maximum y value e.g . if normalizedValue is 0.5 (i*1/seperatorCount) and yMaximum is 100, label will be 50
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();


            // Create Y-axis dashes
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
        }
    }

    private void CreateDotConnection(Vector2 DotPositionA, Vector2 DotPositionB)
    {
        // Create a new GameObject with an Image component to represent the connection between dots
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));

        //set parent to graph container
        gameObject.transform.SetParent(graphContainer, false);

        //set colour of the connection line to white, slightly transparent
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

        Vector2 dir = (DotPositionB - DotPositionA).normalized; //direction from A to B

        float distance = Vector2.Distance(DotPositionA, DotPositionB); //distance between A and B 

        //Makes connection bertween two dots by calculating the direction and distance between them
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        //set size of the connection, width is distance between dots, height is 3
        rectTransform.sizeDelta = new Vector2(distance, 3f);

        //position the connection in the middle of the two dots
        rectTransform.anchoredPosition = DotPositionA + dir * distance * 0.5f;

        //find rotation angle in degrees, takes dir vector and converts to angle
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
}
