using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class ListExtensions
{
    private static readonly System.Random Random = new System.Random();
    public static T GetNextItem<T>(this List<T> list,int currentIndex)
    {
        return currentIndex + 1 >= list.Count-1?list[currentIndex+1]:list[0];
    }
    public static int GetNextIndex<T>(this List<T> list,int currentIndex)
    {
        return currentIndex + 1 < list.Count?currentIndex+1:0;
    }
    public static bool TryGetNextItem<T>(this List<T>list, T item, out T element)
    {
        if (list.Count == 0)
        {
            element = default;
            return false;
        }
        var index = list.IndexOf(item);
        element=index==list.Count - 1?list[0]:list[index + 1];
        return index != list.Count - 1;
    }
    public static bool TryGetPreviousItem<T>(this List<T> list, T item, out T element)
    {
        if (list.Count == 0)
        {
            element = default;
            return false;
        }
        var index = list.IndexOf(item);
        element = index == 0 ? list[0] : list[index - 1];
        return index != 0;
    }
    // public static bool TryGetPreviousIndex<T>(this List<T> list, T item,out int index)
    // {
    //     var currentIndex = list.IndexOf(item);
    //     index=currentIndex == 0 ? currentIndex : currentIndex-1;
    //     return currentIndex != 0;
    // }
    // public static bool TryGetNextIndex<T>(this List<T>list, T item, out int index)
    // {
    //     var currentIndex = list.IndexOf(item);
    //     index=currentIndex==list.Count - 1 ? currentIndex : currentIndex+1;
    //     return currentIndex != list.Count - 1;
    // }
    public static T GetLastElement<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }
    public static List<int> GetThreeCenterIndex<T>(this List<T>list, int index)
    {
        if (list.IsIndexExist(index) == false) return null;
        var newList = new List<int> {index};
        if(list.IsIndexExist(index + 1))newList.Add(index+1);
        if(list.IsIndexExist(index - 1))newList.Add(index-1);
        return newList;
    }
    private static bool IsIndexExist<T>(this List<T>list ,int index)
    {
        return index >= 0 && index < list.Count;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[Random.Next(0,list.Count)];
    }
    public static T GetClosestElementOnZ<T>(this List<T> list, float minDistance, float z)where T:MonoBehaviour
    {
        return list.FirstOrDefault(element=>Mathf.Abs(z-element.transform.position.z)<=minDistance);
    }

    public static T GetMinimalElementOnZ<T>(this List<T>list)where T:MonoBehaviour
    {
        var minimalZ = Mathf.Infinity;
        var minElement = list[0];
        foreach (var element in list)
        {
            var z = element.transform.position.z;
            if(z > minimalZ)continue;
            minimalZ = z;
            minElement = element;
        }
        return minElement;
    }
    public static T GetMaxElementOnZ<T>(this List<T>list)where T:MonoBehaviour
    {
        var maxZ = Mathf.NegativeInfinity;
        var maxElement = list[0];
        foreach (var element in list)
        {
            var z = element.transform.position.z;
            if(z < maxZ)continue;
            maxZ = z;
            maxElement = element;
        }
        return maxElement;
    }
    public static T GetClosestElement<T>(this IEnumerable<T> list, Vector3 point)where T: MonoBehaviour
    {
        var minDistance=Mathf.Infinity;
        T minPoint=null;
        foreach (var element in list)
        {
            var distance = Vector3.Distance(element.transform.position,point);
            if (distance > minDistance) continue;
            minDistance = distance;
            minPoint = element;
        }
        return minPoint;
    }
    public static Vector3 GetClosest(this IEnumerable<Vector3> list, Vector3 point)
    {
        var minDistance=Mathf.Infinity;
        var minPoint=Vector3.one;
        foreach (var element in list)
        {
            var distance = Vector3.Distance(element,point);
            if (distance > minDistance) continue;
            minDistance = distance;
            minPoint = element;
        }
        return minPoint;
    }
}
