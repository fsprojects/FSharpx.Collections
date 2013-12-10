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
			// Create an empty PersistentVector
		    var v = PersistentVector<int>.Empty();
		    // [/create-vector]
		}
	}
}
