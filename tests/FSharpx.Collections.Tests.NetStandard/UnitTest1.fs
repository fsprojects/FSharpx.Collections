namespace FSharpx.Collections.Tests.NetStandard.Tests

open NUnit.Framework

[<TestClass>]
type TestClass () =

    [<SetUp>]
    member this.Setup () =
        ()

    [<Test>]
    member this.Test1 () =
        Assert.Pass()
