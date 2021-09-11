using System;
using FSharpx.Collections;
using System.Runtime.CompilerServices;

namespace CSharp
{
	class PersistentHashMapSamples
	{
		public static void Samples()
		{
			// ------------------------------------------------------------
			// Creating PersistentVectors
			// ------------------------------------------------------------

			// [create-hashmap]
            // Create an empty PersistentHashMap and add some elements
            PersistentHashMap<int,string> map =
                PersistentHashMap<int, string>.Empty()
                    .Add(42,"hello")
                    .Add(99, "world");

            Console.WriteLine(map[42]); // hello
            Console.WriteLine(map[99]); // world

            // Check no. of elements in the PersistentHashMap
            Console.WriteLine(map.Length);  // 2

            // [/create-hashmap]

            // [modify-hashmap]
            PersistentHashMap<int, string> map2 = 
                map
                 .Add(104, "!")
                 .Add(42, "hi");  // replace existing value

            Console.WriteLine(map2[42]); // hi
            Console.WriteLine(map[42]);  // hello            

            Console.WriteLine(map.Length);  // 2
            Console.WriteLine(map2.Length); // 3

            // remove the last element from a PersistentHashMap
            PersistentHashMap<int, string> map3 = map2.Remove(104);

            Console.WriteLine(map3.Length); // 2

            // [/modify-hashmap]
		}
	}
}
