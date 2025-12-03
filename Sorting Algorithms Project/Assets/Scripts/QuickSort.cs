using UnityEngine;

public class QuickSort : MonoBehaviour
{
    public WindowGraph windowGraph;
    public List<int> valueList;
    private int iterations = 1/2;

    void QSort()
    {
        int startPivot = valueList.Count/2;
        PivotSubSort(startPivot);
        int leftPivot = startPivot/2
        int rightPivot = startPivot + leftPivot
        iterations = iterations * (1/2)
        int pivotRange = valueList.Count * iterations
        PivotSubSort(leftPivot);
        PivotSubSort(rightPivot);
    }

    void PivotSubSort(int pivot)
    { //add if pivotRange = 1 or 0
        for (i = 0; i < pivot; i++)
        {
            if (valueList[i] > valueList[pivot])
            {
                int temp = valueList[i]
                valueList.RemoveAt(i)
                valueList.Insert(pivot + 1, temp)
            }
        }
        for (i = pivot + 1; i < valueList.Count; i++)
        {
            if (valueList[i] < valueList[pivot])
            {
                int temp = valueList[i]
                valueList.RemoveAt(i)
                valueList.Insert(pivot - 1, temp)
            }
        }
}
