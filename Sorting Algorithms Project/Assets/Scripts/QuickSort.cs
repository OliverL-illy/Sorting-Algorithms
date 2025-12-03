using System.Collections.Generic;
using UnityEngine;

public class QuickSort : MonoBehaviour
{
    public WindowGraph windowGraph;
    public List<int> valueList;
    public int arrayCount;
    private int leftIterations = 2;
    private int rightIterations = 2;
    int leftPivot;
    int rightPivot;

    void QSort()
    {
        PivotStart(valueList.Count / 2);
        windowGraph.ShowGraph(valueList, -1, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }
    void PivotStart(int pivot)
    {
        int leftPivotRange = valueList.Count / leftIterations;

        if (leftPivotRange > 0)
        {
            for (int i = (pivot - (leftPivotRange / 2)); i < pivot; i++)
            {
                if (valueList[i] > valueList[pivot])
                {
                    int temp = valueList[i];
                    valueList.RemoveAt(i);
                    valueList.Insert(pivot + 1, temp);
                }
            }

            if (pivot / 2 > 0)
            {
                leftPivot = pivot / 2;
                leftIterations *= 2;
                PivotStart(leftPivot);
            }
        }

        int rightPivotRange = valueList.Count / rightIterations;

        if (rightPivotRange > 0)
        {
            for (int i = pivot + 1; i < (pivot + (leftPivotRange / 2)); i++)
            {
                if (valueList[i] < valueList[pivot])
                {
                    int temp = valueList[i];
                    valueList.RemoveAt(i);
                    valueList.Insert(pivot, temp);
                }
            }
        }
        if (rightPivotRange / 2 > 0)
        {
            rightPivot = pivot + (leftPivotRange / 2);
            PivotStart(rightPivot);
            rightIterations *= 2;
        }
    }
}
