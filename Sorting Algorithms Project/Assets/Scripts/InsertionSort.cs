using UnityEngine;

public class InsertionSort : MonoBehaviour
{
    public void ISort(List<int> valueList)
    {
        for (i = 0, i < valueList.count, i++)
        {
            if (valueList[i + 1] < valueList[i])
            {
                j = i
                while (valueList[i + 1] < valueList[j)
                {
                    int endPos = j + 1
                    j--
                }
                int value = valueList[i + 1]
                valueList.RemoveAt(i + 1)
                valueList.Insert(endPos, value)
            }
        }
    }
}
