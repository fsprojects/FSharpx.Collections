PersistentVector
================

A Vector is a collection of values indexed by contiguous integers. Vectors support access to items by index in log32N hops. Count is O(1). Conj puts the item at the end of the vector.

    [lang=csharp,file=csharp/PersistentVector.cs,key=create-vector]

PersistentVectors are immutable and therefor allow to create new version without destruction of the old ones.

    [lang=csharp,file=csharp/PersistentVector.cs,key=modify-vector]
