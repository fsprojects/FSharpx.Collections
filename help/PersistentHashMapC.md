PersistentHashMap
================

A Map is a collection that maps keys to values. Hash maps require keys that correctly support GetHashCode and Equals. Hash maps provide fast access (log32N hops). Count is O(1).

    [lang=csharp,file=csharp/PersistentHashMap.cs,key=create-hashmap]

PersistentHashMaps are immutable and therefor allow to create new version without destruction of the old ones.

    [lang=csharp,file=csharp/PersistentHashMap.cs,key=modify-hashmap]
