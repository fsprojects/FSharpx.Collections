PersistentVector
================

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. count is O(1). conj puts the item at the end of the vector.

    [lang=csharp,file=csharp/PersistentVector.cs,key=create-vector]