module Day7

open System
open System.Linq
open System.Text.RegularExpressions

let abbaRegex = new Regex(@"(.)(.)\2\1")
let abaRegex = new Regex(@"(.)(.)\1")

let private processInput =
    Common.readAsLines "Day7.input.txt"
    |> Array.map (fun s -> s.Split('[', ']'))

let hasAbba str =
    let matches = 
        let rec loop x =
            seq {
                let m = abbaRegex.Match(str, x) 
                if m.Success then
                    yield (m.Groups.[1].Value, m.Groups.[2].Value)
                    yield! loop (m.Index + 1)
            }
        loop 0
    
    matches
    |> Seq.exists (fun (a, b) -> a <> b)

let isTls strings =
    let oddStrings = strings |> Seq.indexed |> Seq.where (fun (n, _) -> n % 2 = 0) |> Seq.map (fun (_, s) -> s)
    let evenStrings = strings |> Seq.indexed |> Seq.where (fun (n, _) -> n % 2 = 1) |> Seq.map (fun (_, s) -> s)
    Seq.exists hasAbba oddStrings && not(Seq.exists hasAbba evenStrings)

let getAbas (str:string) =
    let matches = 
        let rec loop x =
            seq {
                let m = abaRegex.Match(str, x) 
                if m.Success then
                    yield (m.Groups.[1].Value, m.Groups.[2].Value)
                    yield! loop (m.Index + 1)
            }
        loop 0
    
    matches
    |> Seq.where (fun (a, b) -> a <> b)
    |> Seq.distinct

let isSsl strings =
    let oddStrings = strings |> Seq.indexed |> Seq.where (fun (n, _) -> n % 2 = 0) |> Seq.map (fun (_, s) -> s)
    let evenStrings = strings |> Seq.indexed |> Seq.where (fun (n, _) -> n % 2 = 1) |> Seq.map (fun (_, s) -> s)
    oddStrings
    |> Seq.map getAbas
    |> Seq.collect id
    |> Seq.exists (fun (a, b) -> evenStrings |> Seq.exists (fun s -> s.Contains(b + a + b)))

let part1 =
    processInput
    |> Seq.map isTls
    |> Seq.filter id
    |> Seq.length

let part2 =
    processInput
    |> Seq.map isSsl
    |> Seq.filter id
    |> Seq.length
