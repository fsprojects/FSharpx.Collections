using System;
using FSharpx.Collections;
using System.Runtime.CompilerServices;

namespace CSharp
{
	class PersistentVectorSamples
	{
		public static void Samples([CallerFilePath] string file = "")
		{
			// ------------------------------------------------------------
			// Creating PersistentVectors
			// ------------------------------------------------------------

			// [create-vector]
            // Create an empty PersistentVector and add some elements
            PersistentVector<string> v =
		        PersistentVector<string>.Empty()
		            .Conj("hello")
		            .Conj("world")
		            .Conj("!");

            Console.WriteLine(v[0]); // hello
            Console.WriteLine(v[2]); // !            

            // Check no. of elements in the PersistentVector
            Console.WriteLine(v.Length);  // 3
		    // [/create-vector]

            // [modify-vector]            
            PersistentVector<string> v2 = v.Conj("!!!").Update(0,"hi");

            Console.WriteLine(v2[0]); // hi
            Console.WriteLine(v[0]);  // hello
            Console.WriteLine(v2[3]); // !!!

            Console.WriteLine(v.Length);  // 3
            Console.WriteLine(v2.Length); // 4
            // [/modify-vector]
		}
	}
}
