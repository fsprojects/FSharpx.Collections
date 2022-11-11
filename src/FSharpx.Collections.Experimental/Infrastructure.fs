// Copyright 2010-2013, as indicated in README.md in the root directory of this distribution.
//
// Licensed under the Apache License, Version 2.0 (the "License")

namespace FSharpx.Collections.Experimental

module internal ListHelpr =

    let rec loop2Array (left: 'T array) right =
        function
        | x when x < 0 -> left, (List.tail right)
        | x ->
            Array.set left x (List.head right)
            loop2Array left (List.tail right) (x - 1)

    let rec loopFromArray frontLen (left: 'T array) right =
        function
        | x when x > frontLen -> right
        | x -> loopFromArray frontLen left (left.[x] :: right) (x + 1)
