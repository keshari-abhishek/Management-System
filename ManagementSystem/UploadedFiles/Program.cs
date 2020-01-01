using System;

namespace ArrayClass
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[,] arr1 = new int[3,3] { { 1, 2, 3}, { 1, 2, 3}, { 9, 8, 7} };
            char[] arr3 = new char[] { 'q', 'w', 'e', 'r' };
            //to check the length
            Console.WriteLine(arr.Length);
            Console.WriteLine(arr1.Length);
            Console.WriteLine(arr3.Length);

            Console.WriteLine(arr.LongLength);
            Console.WriteLine(arr1.LongLength);
            Console.WriteLine(arr3.LongLength);

            //to check that array is of fixed size or not
            Console.WriteLine(arr.IsFixedSize);
            Console.WriteLine(arr1.IsFixedSize);
            Console.WriteLine(arr3.IsFixedSize);

            //to check the read and write operation can be performed or not
            Console.WriteLine(arr.IsReadOnly);
            Console.WriteLine(arr1.IsReadOnly);
            Console.WriteLine(arr3.IsReadOnly);

            //to check the synchronizetion
            Console.WriteLine(arr.IsSynchronized);
            Console.WriteLine(arr1.IsSynchronized);
            Console.WriteLine(arr3.IsSynchronized);

            //to find the rank of array
            Console.WriteLine(arr.Rank);
            Console.WriteLine(arr1.Rank);
            Console.WriteLine(arr3.Rank);

            Console.WriteLine(arr.SyncRoot);
            Console.WriteLine(arr1.SyncRoot);
            Console.WriteLine(arr3.SyncRoot);
            int n = Array.BinarySearch(arr, 5);
            Console.WriteLine("index location of element: "+n);

            


        }
    }
}

