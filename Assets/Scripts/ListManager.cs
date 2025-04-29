using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace MountAndBlade
{
    public static class ListManager<T> where T : Object
    {
        private static List<List<T>> lists = new List<List<T>>();
        private static List<Type> map = new List<Type>();


        public static void Add(T objectToAdd)
        {
            if (map.Contains(typeof(T)))
            {
                lists[map.IndexOf(typeof(T))].Add(objectToAdd);
            }
            else
            {
                lists[map.Count] = new List<T>();
                lists[map.Count].Add(objectToAdd);
                map.Add(typeof(T));
            }
        }

        public static void Remove(T objectToRemove)
        {
            if (map.Contains(typeof(T)))
            {
                lists[map.IndexOf(typeof(T))].Remove(objectToRemove);
                if (lists[map.IndexOf(typeof(T))].Count == 0)
                {
                    lists[map.IndexOf(typeof(T))] = null;
                    map.Remove(objectToRemove.GetType());
                }
            }
        }

        public static List<T> GetList()
        {
            if (map.Contains(typeof(T)))
            {
                return lists[map.IndexOf(typeof(T))];
            }

            return new List<T>();
        }

        public static int GetListCount()
        {
            if (map.Contains(typeof(T)))
            {
                return lists[map.IndexOf(typeof(T))].Count;
            }

            return 0;
        }
    }
}