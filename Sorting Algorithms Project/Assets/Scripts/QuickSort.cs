using UnityEngine;

public class QuickSort : MonoBehaviour
{
    public WindowGraph windowGraph;
    public List<int> valueList;

    void QSort()
    {
        int startPivot = valueList.Count/2;
        PivotSubSort(startPivot);
    }

    void PivotSubSort(int pivot)
    {
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
