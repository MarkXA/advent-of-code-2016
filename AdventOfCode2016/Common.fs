module Common

open System
open System.IO
open System.Reflection
open FsAlg.Generic

let readInput filename =
    let assembly = Assembly.GetExecutingAssembly()
    use stream = assembly.GetManifestResourceStream(filename)
    use reader = new StreamReader(stream)
    let result = reader.ReadToEnd()
    result

let vectorEquals (v1:Vector<float>) (v2:Vector<float>) =
    (int v1.[0] = int v2.[0]) && (int v1.[1] = int v2.[1])
