// Copyright 2010-2013, as indicated in README.md in the root directory of this distribution.
//
// Licensed under the Apache License, Version 2.0 (the "License")

namespace FSharpx.Collections

module Exceptions = 
    let Empty = new System.Exception("Queue is empty") // TODO: make this a better exception

    let OutOfBounds = new System.IndexOutOfRangeException() // TODO: make this a better exception

    let KeyNotFound key = new System.Collections.Generic.KeyNotFoundException("The key " + string key + " was not present in the collection")