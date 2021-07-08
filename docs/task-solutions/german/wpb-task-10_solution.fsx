(**
---
title: 05 Data access
category: Musterlösungen(deutsch)
categoryindex: 1
index: 10
---
*)

(**

[![Binder](/BIO-BTE-12-V-4/img/badge-binder.svg)](https://mybinder.org/v2/gh/csbiology/BIO-BTE-12-V-4/gh-pages?filepath=tasks/german/wpb-task-10.ipynb)&emsp;
[![Script](/BIO-BTE-12-V-4/img/badge-script.svg)](/BIO-BTE-12-V-4/tasks/german/wpb-task-10.fsx)&emsp;
[![Notebook](/BIO-BTE-12-V-4/img/badge-notebook.svg)](/BIO-BTE-12-V-4/tasks/german/wpb-task-10.ipynb)
# Task10 - Datenzugriff
## 0 Vorwort
Folgende Dokumentationen können für die Bearbeitung der Aufgaben hilfreich sein:

* Plotly.NET: https://plotly.net
* Deedle: https://fslab.org/Deedle
* FSharp.Stats: https://fslab.org/FSharp.Stats

### Referenzieren von Plotly.NET
Diese Zeilen müssen immer mindestens einmal ausgeführt werden, sonst können die Softwarepakete nicht verwendet werden:

*)
#r "nuget: Plotly.NET, 2.0.0-preview.4"
#r "nuget: Deedle, 2.3.0"
#r "nuget: FSharp.Stats, 0.4.1"
open Plotly.NET
open Deedle
open FSharp.Stats
(**
Bei dieser Übung unterscheiden sich die Arbeitsweisen bezüglich des Anzeigens der erstellten Diagramme in Notebooks und .fsx Skripten grundlegend:
### Anzeigen von Charts in .fsx Skripten
In .fsx Skripten sollte die `Chart.Show` Funktion verwendet werden, welche ein Browserfenster öffnet um Diagramme anzuzeigen:

*)
Chart.Point([(1,1); (2,2)])
|> Chart.withTitle "hello from .fsx"
|> Chart.Show
(**
### Anzeigen von Charts in Notebooks
In Notebooks kann zwar auch die Chart.Show Funktion verwendet werden, dank der oben referenzierten `Plotly.NET.Interactive` 
Erweiterung kann allerdings auch einfach der jeweilige Codeblock mit dem value des Charts beendet werden (so wie auch bei anderen Werten gewohnt), 
um den Chart direkt in der Ausgabezelle anzuzeigen:

*)
Chart.Point([(1,1); (2,2)])
|> Chart.withTitle "hello inside the notebook"
(**
### Arbeiten mit Deedle
Sollten Sie diese Fehlermeldung sehen:
```
9_Data_exploration_using_FSharp.fsx(113,5): error FS0030: Value restriction. The value 'cpw'' has been inferred to have generic type val cpw' : Series<(string * int),'_a>      
Either define 'cpw'' as a simple data term, make it a function with explicit arguments or, if you do not intend for it to be generic, add a type annotation.
```
Dann sollten Sie auf eine explizite Typenanmerkung zurückgreifen.
Statt:
```
let cpw' = persons |> Frame.getCol "cpw"
```
Verwenden Sie:
```
let cpw' :Series<int,float> = persons |> Frame.getCol "cpw"
```
Zum anzeigen von Deedle-Objekten in diesem Notebook können Sie den `.Print()` Member verwenden:

*)
let firstNames' = Series.ofValues ["Kevin";"Lukas";"Benedikt";"Michael"]
firstNames'.Print()
(**
## 1 Basics
### Task 1.1
Gegeben sind 4 Series gleicher Länge. Nutzen Sie die Funktion `Series.mapValues` um die Werte von "coffeesPerWeek" zu verdoppeln. 

*)
let firstNames      = Series.ofValues ["Kevin";"Lukas";"Benedikt";"Michael"] 
let coffeesPerWeek  = Series.ofValues [15;12;10;11] 
let lastNames       = Series.ofValues ["Schneider";"Weil";"Venn";"Schroda"]  
let group           = Series.ofValues ["CSB";"CSB";"CSB";"MBS"]

let coffeesPerWeekDoubled = Series.mapValues (fun x -> string(x*2)) coffeesPerWeek

(**
### Task 1.2
Erstellen Sie auf Basis der 4 gegebenen Series einen Frame mit dem Namen "persons". 

*)

/// Variante 1
let cpwstring = Series.mapValues (fun a -> string a) coffeesPerWeek 

let persons = Frame.ofColumns ["fn", firstNames; "ln", lastNames; "cpw", cpwstring; "g", group;] 

/// Variante 2
let persons':Frame<int,string> = Frame.ofColumns ["firstNames", firstNames; "lastNames", lastNames; "group", group]

let persons2 = persons' |> Frame.addCol "coffeesPerWeek" coffeesPerWeek

/// Variante 2.5
let persons25   = Frame.ofColumns["vorname",firstNames;"Nachname",lastNames;"Gruppe",group] 
                |> Frame.addCol "CPW" coffeesPerWeek

/// Variante 3
type Person = 
    {FirstNames: string; CoffeesPerWeek: int; LastNames: string; Group: string}

let personsList = 
    [
        [
            for i in [0..3] do 
                {
                    FirstNames = firstNames.[i]
                    CoffeesPerWeek = coffeesPerWeek.[i]
                    LastNames = lastNames.[i]
                    Group = group.[i]
                }
        ]
    ]

let persons3 = Frame.ofRecords personsList

(**
### Task 1.3
Fügen Sie eine Column mit dem Namen "sodasPerWeek" zu dem Frame hinzu. Binden Sie den resultierenden Frame an einen Namen.
Tipp: Erst eine `Series<int,int>` erstellen. Nutzen Sie `Frame.addCol`
*)

let sodasPerWeek = Series.ofValues[20;12;6;40] // Hinzufügen einer weiteren Series
let personsWithSoda = Frame.addCol "Spw" sodasPerWeek persons
personsWithSoda.Print()

(**
### Task 1.4
Addieren Sie die Columns "sodasPerWeek" und "coffeesPerWeek". Fügen Sie die resultierende Series als Spalte mit dem Titel "allPurchases" zu dem zuvor erstellten Frame hinzu.
Tipp 1: Diese Task kann auf mehrere Arten und Weisen gelöst werden.
Tipp 2: Via `Series.values` können Sie auf die Werte der einzelnen Series zugreifen. Dann könnten Sie mit `Seq.map2` über beide Collections iterieren. 
*)


/// Variante 1

let allPurchases = 
    Seq.map2 (fun a b -> a + b) (Series.values sodasPerWeek) (Series.values coffeesPerWeek)
    |> Series.ofValues

let personsWithAllPurchases = Frame.addCol "allPurchases" allPurchases persons


/// Variante 2

let allPurchases2 = sodasPerWeek + coffeesPerWeek

let personsWithAllPurchases2 = Frame.addCol "allPurchases" allPurchases2 persons

 
/// Variante 3
let soda = Series.values sodasPerWeek |> Seq.toArray
let coffee = Series.values coffeesPerWeek |> Seq.toArray
let allPurchases3 = 
    seq {
        for i = 0 to (soda.Length-1) do
            soda.[i] + coffee.[i]
    } 
    |> Series.ofValues

let personsWithAllPurchases3 = Frame.addCol "allPurchases"  allPurchases3 persons


(**
### Task 1.5
Bestimmen Sie die Summe von "allPurchases".
*)

/// Variante 1

allPurchases
|> Series.values 
|> Seq.sum

/// Variante 2

Stats.sum allPurchases


(**
## 2 Frame Operationen
### Task 2.1
Gruppieren Sie die Zeilen des Frames aus Task 1.2 nach den Elementen der Spalte "group".
Tipp: Explizite Typenanmerkung (siehe: [Arbeiten mit Deedle](#Arbeiten-mit-Deedle)) 
*)

let groupedByG :Frame<string*int,string> = persons |> Frame.groupRowsBy "g"

(**
### Task 2.2
Oft enthalten Ergebnistabellen mehr als 40 Spalten. Für einzelne Analysen sind jedoch nur einige wenige interessant. 
Es bietet sich daher oft an einen Frame zu erstellen, der weniger Spalten enthält. Nutzen Sie die Funktion `Frame.sliceCols` um auf 
Basis des Frames aus Task 1.2 einen Frame zu erstellen, der lediglich die Spalten "lastNames" und "coffeesPerWeek" enthält. 
*)

let withoutFnAndG :Frame<string*int,string> = groupedByG |> Frame.sliceCols ["fn";"cpw";]

(**
### Task 2.3
Oft möchte man auf Basis von Gruppierungen aggregieren. Berechnen Sie die Summe der Spalte "coffeePerWeek" für jede Gruppe.
Tipp: Extrahieren Sie die Spalte "coffeePerWeek" aus dem Ergebnis von Aufgabe 2.1. Verfahren Sie wie in der Vorlesung demonstriert. 
*)

let coffeePerWeek'' :Series<string*int,int> = groupedByG |> Frame.getCol ("cpw")

let coffeePerWeekPerGroup =  
    // Series.applyLevel fst (Series.values >> Seq.sum) coffeePerWeek''
    Series.applyLevel Pair.get1Of2 (Series.values >> Seq.sum) coffeePerWeek''


(**
### Task 2.4
Oft möchte man Zwischenergebnisse abspeichern. Speichern Sie den Frame aus Aufgabe 1.2 als CSV Datei. Verwenden Sie ';' als Trennzeichen. 
*)

let personPath = @"C:\Users\BesterNutzer\BesterKurs\BesterFrame.csv"

persons.SaveCsv(personPath, separator = ';', includeRowKeys = false)

(**
### Task 2.5
Verwenden Sie die Funktion `Frame.ReadCsv` um die Datei erneut einzulesen. 
*)

Frame.ReadCsv(personPath, separators = ";")

(**
*)

