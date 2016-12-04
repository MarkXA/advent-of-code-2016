module Day4

open System
open System.Text.RegularExpressions

let private processInput =
    let input = Common.readAsLines "Day4.input.txt"
    let regex = new Regex("^(.*)-([0-9]+)\\[([a-z]+)\\]")
    let rooms =
        input
        |> Array.map (fun s -> regex.Match(s).Groups)
        |> Array.map (fun g -> (g.[1].Value.ToCharArray(), int g.[2].Value, g.[3].Value))
    rooms

let mostCommonLetters chars =
    chars
    |> Array.filter Char.IsLetter
    |> Array.groupBy id
    |> Array.map (fun (letter, group) -> (letter, group.Length))
    |> Array.sortBy (fun (letter, number) -> (-number, letter))
    |> Array.map (fun (letter, number) -> letter)

let roomCode chars =
    let top5 =
        chars
        |> mostCommonLetters
        |> Array.take 5
    new String(top5)

let shiftChar number c =
    match c with
    | '-' -> ' '
    | _ -> char (((int c - int 'a' + number) % 26) + int 'a')

let decrypt number chars =
    let decrypted = chars |> Array.map (shiftChar number)
    new String(decrypted)

let part1 =
    let matches =
        processInput
        |> Array.map (fun (room, number, code) -> (number, code, roomCode room))
        |> Array.filter (fun (_, code1, code2) -> code1 = code2)
        |> Array.map (fun (number, _, _) -> number)
    matches |> Array.sum

let part2 =
    let matchingRoom =
        processInput
        |> Array.map (fun (room, number, code) -> (room, number, code, roomCode room))
        |> Array.filter (fun (_, _, code1, code2) -> code1 = code2)
        |> Array.map (fun (room, number, _, _) -> (decrypt number room, number))
        |> Array.find (fun (name, number) -> name = "northpole object storage")
    snd matchingRoom
