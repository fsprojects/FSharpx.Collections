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
            PersistentVector<string> vector =
                PersistentVector<string>.Empty()
                    .Conj("hello")
                    .Conj("world")
                    .Conj("!");

            Console.WriteLine(vector[0]); // hello
            Console.WriteLine(vector[2]); // !            

            // Check no. of elements in the PersistentVector
            Console.WriteLine(vector.Length);  // 3

		    // [/create-vector]

            // [modify-vector]
            PersistentVector<string> vector2 = vector.Conj("!!!").Update(0,"hi");

            Console.WriteLine(vector2[0]); // hi
            Console.WriteLine(vector[0]);  // hello
            Console.WriteLine(vector2[3]); // !!!

            Console.WriteLine(vector.Length);  // 3
            Console.WriteLine(vector2.Length); // 4

            // remove the last element from a PersistentVector
            PersistentVector<string> vector3 = vector2.Initial;

            Console.WriteLine(vector3.Length); // 3

            // [/modify-vector]
		}
	}
}
