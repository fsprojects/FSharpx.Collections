module FSharpx.Collections.TimeMeasurement

/// Stops the runtime for a given function
let stopTime f = 
    let sw = new System.Diagnostics.Stopwatch()
    sw.Start()
    let result = f()
    sw.Stop()
    result,float sw.ElapsedMilliseconds

/// Stops the average runtime for a given function and applies it the given count
let stopAverageTime count f = 
    System.GC.Collect() // force garbage collector before testing
    let sw = new System.Diagnostics.Stopwatch()
    sw.Start()
    for _ in 1..count do
        f() |> ignore

    sw.Stop()
    float sw.ElapsedMilliseconds / float count

let printInFsiTags s = printfn " [fsi:%s]" s

/// Stops the average runtime for a given function and applies it the given count
/// Afterwards it reports it with the given description
let stopAndReportAvarageTime count desc f =
    let time = stopAverageTime count f
    sprintf "%s %Ams" desc time |> printInFsiTags