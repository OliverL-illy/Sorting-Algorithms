using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite dotSprite;

    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private List<GameObject> gameObjectList;

    private void Awake()
    {
        graphContainer = transform.Find("Graph Container").GetComponent<RectTransform>();

        labelTemplateX = graphContainer.Find("Label Template X").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("Label Template Y").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("Dash Template X").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("Dash Template Y").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33};
        ShowGraph(valueList, -1, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f)); // axis labels set to empty for simplicity for now, _i + 1 to make x axis start from 1 instead of 0 (only visually)
    }

    // Method to create a dot GameObject at a specified anchored position
    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        // Create a new GameObject with an Image component to represent the dot
        GameObject gameObject = new GameObject("dot", typeof(Image));

        // Set the parent of the GameObject to the graph container
        gameObject.transform.SetParent(graphContainer, false);

        // Set the sprite of the Image component to the dot sprite
        gameObject.GetComponent<Image>().sprite = dotSprite;

        // Get the RectTransform of the GameObject
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        // Set the anchored position, size, and anchors of the RectTransform, In this case, size is set to 11x11 and anchors to bottom-left
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    // ShowGraph(list of values to show, x axis labels , y axis labels) (= null to set as optional)
    private void ShowGraph(List<int> valueList, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        //If want to customise x and y axis labels, use this video as reference: https://www.youtube.com/watch?v=3ozu5osNw-I
        //doesnt do anything right now, optional
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if(maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count; // Show all values if maxVisibleValueAmount is not set or invalid
        }

        // Clear any existing graph elements
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        //height of the graph container (sizeDelta.y gets the height of the RectTransform)
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x; // width of the graph container

        //maxVisibleValueAmount = Maximum number of data points to display on the graph

        //maximum and minimum y values to scale the graph accordingly
        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        // Find the maximum value in the valueList to scale the y positions accordingly, only considering the last 'maxVisibleValueAmount' values if there are more values than that
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)// Mathf.Max to ensure start index is not negative, picks the higher of the two values
        {
            int value = valueList[i];

            if (value > yMaximum)
            {
                yMaximum = value;
            }

            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum; // Difference between max and min y values
        if (yDifference <= 0f)
        {
            yDifference = 5f; // Prevent division by zero later on if only one point, set a default difference
        }

        // Add some padding to the y-axis range for better visualization, 20% of difference between max and min
        yMaximum = yMaximum + yDifference * 0.2f; 
        yMinimum = yMinimum - yDifference * 0.2f; 

        yMinimum = 0f; // Optional: Uncomment this line if you want the y-axis to always start at 0

        float xSize = graphWidth/(maxVisibleValueAmount + 1); // Spacing between each data point on the x-axis, calculated based on the graph width and maximum visible values so they always fit within the graph. +1 for slight offset from edge

        int xIndex = 0; // Works as i in for loop but only to position visible points (first visible point is at xIndex 0, second at xIndex 1, etc)

        GameObject lastDotGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            // Calculate the x and y position for each data point, x is spaced by xSize + (for offset), y is scaled based on the maximum value
            float xPosition = xSize + xIndex * xSize;
            // yPosition is calculated by normalizing the value to the maximum (we do this to get a value between 0 and 1, a ratio of y-maximum) and then multiplying by the graph height to get the actual position in the graph
            //float yPosition = (valueList[i] / yMaximum) * graphHeight;
            // Updated to account for minimum y value as well, so that the graph can handle negative values too
            float yPosition = (valueList[i] - yMinimum) / (yMaximum - yMinimum) * graphHeight;

            // Create a dot at the calculated position
            GameObject dotGameObject = CreateDot(new Vector2(xPosition, yPosition));

            gameObjectList.Add(dotGameObject); // Add the created dot to the list for future reference, if need to clear later for new graph

            // If there is a last dot, create a connection between the last dot and the current dot. (if statement is to skip the first point since there is no last point to connect to)
            if (lastDotGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject); // Add the created connection to the list for future reference
            }
            // Update lastDotGameObject to the current dot for the next iteration
            lastDotGameObject = dotGameObject;

            // Create X-axis labels
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject); // Add the created label to the list for future reference

            // Create X-axis dashes
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -20f);
            gameObjectList.Add(dashX.gameObject); // Add the created dash to the list for future reference

            xIndex++;
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
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum))); // Updated to account for minimum y value as well e.g. if yMinimum is -50 and yMaximum is 100, at normalizedValue 0.5 (halfway through y axis), label will be 25
            gameObjectList.Add(labelY.gameObject); // Add the created label to the list for future reference

            // Create Y-axis dashes
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject); // Add the created dash to the list for future reference
        }
    }

    private GameObject CreateDotConnection(Vector2 DotPositionA, Vector2 DotPositionB)
    {
        // Create a new GameObject with an Image component to represent the connection between dots
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));

        //set parent to graph container
        gameObject.transform.SetParent(graphContainer, false);

        //set colour of the connection line to white, slightly transparent
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

        Vector2 dir = (DotPositionB - DotPositionA).normalized; //direction from A to B

        float distance = Vector2.Distance(DotPositionA, DotPositionB); //distance between A and B 

        //Makes connection between two dots by calculating the direction and distance between them
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        //set size of the connection, width is distance between dots, height is 3
        rectTransform.sizeDelta = new Vector2(distance, 3f);

        //position the connection in the middle of the two dots
        rectTransform.anchoredPosition = DotPositionA + dir * distance * 0.5f;

        //find rotation angle in degrees, takes dir vector and converts to angle
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));

        return gameObject; // return the created connection GameObject
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
