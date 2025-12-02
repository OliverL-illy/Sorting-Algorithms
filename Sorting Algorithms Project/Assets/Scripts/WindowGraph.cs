using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private List<GameObject> gameObjectList;
    public List<int> valueList = new List<int>();
    [SerializeField] int maxListNum = 1024;

    private void Awake()
    {
        graphContainer = transform.Find("Graph Container").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("Label Template X").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("Label Template Y").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("Dash Template X").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("Dash Template Y").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        // Fill valueList with random test data
        for (int i = 0; i < maxListNum; i++)
        {
            valueList.Add(UnityEngine.Random.Range(0, 200));
        }

        ShowGraph(valueList);
    }

    public void ShowGraph(List<int> values, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (values == null || values.Count == 0) return;

        if (maxVisibleValueAmount <= 0 || maxVisibleValueAmount > values.Count)
            maxVisibleValueAmount = values.Count;

        getAxisLabelX ??= (i => (i + 1).ToString());
        getAxisLabelY ??= (f => Mathf.RoundToInt(f).ToString());

        // Clear previous bars and labels
        foreach (var go in gameObjectList) Destroy(go);
        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        // Find min/max values
        float yMax = values[0];
        float yMin = values[0];
        for (int i = values.Count - maxVisibleValueAmount; i < values.Count; i++)
        {
            if (values[i] > yMax) yMax = values[i];
            if (values[i] < yMin) yMin = values[i];
        }

        float yDiff = yMax - yMin;
        if (yDiff <= 0f) yDiff = 5f; // prevent divide by zero

        // Optional padding
        yMax += yDiff * 0.2f;
        yMin = 0f; // optional: always start from 0

        // Calculate exact bar width
        float barWidth = (graphWidth / maxVisibleValueAmount);

        for (int i = 0; i < maxVisibleValueAmount; i++)
        {
            int valueIndex = values.Count - maxVisibleValueAmount + i;
            float xPos = i * barWidth + barWidth / 2f; // center of bar
            float yPos = (values[valueIndex] - yMin) / (yMax - yMin) * graphHeight;

            GameObject bar = CreateBar(new Vector2(xPos, yPos), barWidth);
            gameObjectList.Add(bar);

            // Optional: X-axis dash
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.anchoredPosition = new Vector2(xPos, -20f);
            gameObjectList.Add(dashX.gameObject);
        }

        // Y-axis labels/dashes
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            float normalizedValue = i / (float)separatorCount;
            float yPos = normalizedValue * graphHeight;

            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.anchoredPosition = new Vector2(-7f, yPos);
            labelY.gameObject.SetActive(true);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMin + normalizedValue * (yMax - yMin));
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.anchoredPosition = new Vector2(-4f, yPos);
            dashY.gameObject.SetActive(true);
            gameObjectList.Add(dashY.gameObject);
        }
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth)
    {
        GameObject go = new GameObject("bar", typeof(Image));
        go.transform.SetParent(graphContainer, false);

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rect.sizeDelta = new Vector2(barWidth, graphPosition.y);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0.5f, 0f);

        return go;
    }
}
