module FSharpx.Collections.Literals

[<Literal>]
let internal blockSizeShift = 5 // TODO: what can we do in 64Bit case?

[<Literal>]
let internal blockSize = 32

[<Literal>]
let internal blockIndexMask = 0x01f