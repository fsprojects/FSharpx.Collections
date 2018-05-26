namespace FSharpx.Collections.Experimental.Tests

// originally from http://learnyouahaskell.com/zippers

open System
open FSharpx
open FSharpx.Collections.Experimental
open FSharpx.Collections
open Expecto
open Expecto.Flip

module FileSystemZipperTest =

    type FileName = string

    type Folder = { Name: FileName; Items: FSItem list }
    and FSItem =
        | File of FileName
        | Folder of Folder

    let folder(name,items) = Folder { Name = name ; Items = items }

    type Path = FileName * FSItem list * FSItem list

    type FSZipper = { Focus : FSItem; Path : Path option }

    /// Moves the zipper one directory up
    let up (zipper:FSZipper) : FSZipper = 
   
        match zipper.Path with
        | Some (name,ls,rs) -> { zipper with Focus = Folder { Name = name; Items = ls @ [zipper.Focus] @ rs} }

    let getName = function
        | File fileName -> fileName
        | Folder folder -> folder.Name  

    let nameIs name item = getName item = name

    /// Moves down to the folder with the given name
    let moveTo name (zipper:FSZipper) : FSZipper = 
        match zipper.Focus with
        | Folder folder ->
            let (ls, item::rs) = List.split (nameIs name) folder.Items
            { Focus = item; Path = Some(folder.Name,ls,rs) }
 
    /// Renames the given focus
    let rename newName zipper =
        match zipper.Focus with
        | File name -> { zipper with Focus = File newName }
        | Folder folder -> { zipper with Focus = Folder { folder with Name = newName } }

    /// Creates a new file in the current directory
    let newFile name zipper =
        match zipper.Focus with
        | Folder folder -> { zipper with Focus = Folder { folder with Items = File name :: folder.Items } }

    /// Creates a new folder in the current directory
    let newFolder name zipper =
        match zipper.Focus with
        | Folder folder -> { zipper with Focus = Folder { folder with Items = Folder {Name = name; Items = [] } :: folder.Items } }

    let zipper fileSystem : FSZipper = { Focus = fileSystem; Path = None }

    let disk =
        folder("root",
            [File "goat_yelling_like_man.wmv"
             File "pope_time.avi"
             folder("pics",
                [File "ape_throwing_up.jpg"
                 File "watermelon_smash.gif"
                 File "skull_man(scary).bmp"])
             File "dijon_poupon.doc"
             folder("programs",
                [File "fartwizard.exe"
                 File "owl_bandit.dmg"
                 File "not_a_virus.exe"
                 folder("source code",
                    [File "best_hs_prog.hs"
                     File "random.hs"])
                ])
            ]) |> zipper

    [<Tests>]
    let testFileSystemZipper =

        testList "Experimental FileSystemZipper" [
            test "Can move to subdir" {
                let z = disk |> moveTo "pics" |> moveTo "skull_man(scary).bmp"  
                Expect.equal "" (File "skull_man(scary).bmp") z.Focus }

            test "Can move to subdir and up again" {
                let z = disk |> moveTo "pics" |> moveTo "skull_man(scary).bmp" |> up
                Expect.equal "" "pics" <| getName z.Focus }

            test "Can rename a folder" {
                let z = disk |> moveTo "pics" |> rename "photo" |> up |> moveTo "photo"
                Expect.equal "" "photo" <| getName z.Focus }

            test "Can rename a file" {
                let z = disk |> moveTo "pics" |> moveTo "skull_man(scary).bmp"  |> rename "scary.bmp" |> up |> moveTo "scary.bmp"
                Expect.equal "" "scary.bmp" <| getName z.Focus }

            test "Can't access a renamed file with the old name" {
                let ok = ref false
                let z1 = disk |> moveTo "pics" |> moveTo "skull_man(scary).bmp"  |> rename "scary.bmp" |> up 
                try
                  z1 |> moveTo "skull_man(scary).bmp"
                  ()
                with 
                | exn -> ok := true
                Expect.isTrue "" !ok }

            test "Can create a new file" {
                let z = disk |> moveTo "pics" |> newFile "scary.bmp" |> moveTo "scary.bmp"
                Expect.equal "" "scary.bmp" <| getName z.Focus }

            test "Can still access old files if a new one is created" {
                let z = disk |> moveTo "pics" |> newFile "scary.bmp" |> moveTo "skull_man(scary).bmp"
                Expect.equal "" "skull_man(scary).bmp" <| getName z.Focus }

            test "Can create a new folder" {
                let z = disk |> moveTo "pics" |> newFolder "wedding" |> moveTo "wedding"
                Expect.equal "" (Folder { Name = "wedding"; Items = []}) z.Focus }
        ]