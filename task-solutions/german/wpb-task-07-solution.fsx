(**
// can't yet format YamlFrontmatter (["title: 07 Datavisualization with Plotly.NET"; "category: Musterlösungen(deutsch)"; "categoryindex: 1"; "index: 5"], Some { StartLine = 2 StartColumn = 0 EndLine = 6 EndColumn = 8 }) to pynb markdown

[![Binder](/BIO-BTE-12-V-4-SOLUTIONS/img/badge-binder.svg)](https://mybinder.org/v2/gh/csbiology/BIO-BTE-12-V-4/gh-pages?filepath=task-solutions/german/wpb-task-07-solution.ipynb)&emsp;
[![Script](/BIO-BTE-12-V-4-SOLUTIONS/img/badge-script.svg)](/BIO-BTE-12-V-4-SOLUTIONS/task-solutions/german/wpb-task-07-solution.fsx)&emsp;
[![Notebook](/BIO-BTE-12-V-4-SOLUTIONS/img/badge-notebook.svg)](/BIO-BTE-12-V-4-SOLUTIONS/task-solutions/german/wpb-task-07-solution.ipynb)

# Datenvisualisierung mit Plotly.NET

**Verwenden Sie fuer alle Diagramme stets adequate Skalenbeschriftungen.**

**Der Code fuer die Erstellung der Charts reicht aus, Sie muessen keine Bilder einreichen.**

## 0 Vorwort

Die Plotly.NET Dokumentation finden sie hier: https://plotly.net

### Referenzieren von Plotly.NET

Diese Zeilen müssen immer mindestens 1 mal ausgeführt werden, sonst kann das Plotly.NET Softwarepaket nicht verwendet werden:

*)
#r "nuget: Plotly.NET, 2.0.0-preview.1"
#r "nuget: Plotly.NET.Interactive, 2.0.0-preview.1"
open Plotly.NET
(**
Bei dieser Uebung unterscheiden sich die Arbeitsweisen bezueglich des Anzeigens der erstellten Diagramme in Notebooks und .fsx Skripten grundlegend:

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
## 1 Visualisierung von X/Y Beziehungen

*)
//Ich erstelle mir generell immer ein paar funktionen zum stylen von Charts. Ich mag zum beispiel gespiegelte achsen, die den plot einrahmen.
//diese funktion kann ich dann für alle Charts im projekt benutzen. Das war natürlich nicht von euch verlangt, ich packe es aber als
//"real world" example dazu.

let myLinearAxis title showGrid = 
    Axis.LinearAxis.init(
        Title = title,
        //Zeige hilfslinien nur wenn ich das sage
        Showgrid=showGrid,
        //Achse als durchgezogene linie
        Showline=true,
        //spiegelt die Achsen auf beiden seiten des plots
        Mirror=StyleParam.Mirror.All,
        Tickmode=StyleParam.TickMode.Auto,
        //Striche der Skala sind im Plot
        Ticks= StyleParam.TickOptions.Inside, 
        //Stylen des titels
        Tickfont=Font.init(StyleParam.FontFamily.Arial,Size=16.),
        Titlefont=Font.init(StyleParam.FontFamily.Arial,Size=20.)
    )
(**
### Task 1.1

Erstellen Sie eine Liste, welche die Werte von 0.0 bis (4 * PI) mit einer Schrittgroesse von 0.001 enthaelt.
binden Sie diese liste an den Namen 'xValues'


*)
let xValues = [0.0 .. 0.001 .. 4. * System.Math.PI]
(**
### Task 1.2

Erstellen Sie zwei Listen, die die passenden Funktionswerte von sin(x) und cos(x) fuer 'xValues' enthalten.

_Tipp: benutzen Sie die map Funktion_

*)
let sinValues =
    xValues
    |> List.map sin

let cosValues = 
    xValues
    |> List.map cos
(**
### Task 1.3

Erstellen Sie zwei Punkt- und zwei Liniendiagramme in denen Sie die Werte von 'xValues' und die jeweiligen
Funktionswerte von sin(x) oder cos(x) auf je eine Positionsskala abbilden. Am Ende sollten Sie 4 einzelne 
Diagramme haben.

*)
//Da ihr die charts 2 mal verwenden musstet (einzeln und kombiniert), macht es sinn sie an einen namen zu binden
//um sie wiederverwenden zu können.

let sinLines = 
    Chart.Line(xValues,sinValues,Name="sinLines")
    |> Chart.withX_AxisStyle "x"
    |> Chart.withY_AxisStyle "sin(x)"
    |> Chart.withTitle "sin (x)"

//hier könnt ihr die custom achse von oben in action sehen.
//Da es sich um einen Axis.LinearAxis typ handelt, benutze ich hier withY_Axis anstatt withY_AxisStyle
let cosLines = 
    Chart.Line(xValues,cosValues,Name="cosLines")
    |> Chart.withY_Axis (myLinearAxis "x" false)
    |> Chart.withX_Axis (myLinearAxis "cos(x)" false)
    |> Chart.withTitle "cos (x)"   

let sinPoints = 
    Chart.Point(xValues,sinValues,Name="sinPoints")
    |> Chart.withX_AxisStyle "x"
    |> Chart.withY_AxisStyle "sin(x)"
    |> Chart.withTitle "sin (x)"

let cosPoints = 
    Chart.Point(xValues,cosValues,Name="cosPoints")
    |> Chart.withX_AxisStyle "x"
    |> Chart.withY_AxisStyle "cos(x)"
    |> Chart.withTitle "cos (x)"
(**
### Task 1.4

**a)** Kombinieren Sie die Punkt- und Liniendiagramme aus 1.3. in einem einzigen Koordinatensystem (zu einem einzigen Diagramm mit 4 Unterdiagrammen). 

_Tipp: Benutzen Sie Chart.Combine_

**b)** Warum ist das Resultat ein Diagramm mit einer zusaetzlichen Dimension, bzw. was ist die zusätzlich abgebildete Dimension?

*)
//Die punkte sind mir in dem plot zu gro?, also nutze ich Chart.withMarkewrStyle um sie zu verändern.

[
    sinPoints
    |> Chart.withMarkerStyle(5,Symbol = StyleParam.Symbol.Cross)
    cosPoints
    |> Chart.withMarkerStyle(5,Symbol = StyleParam.Symbol.Cross)
    sinLines
    cosLines
]
|> Chart.Combine
|> Chart.withX_Axis (myLinearAxis "x" false)
|> Chart.withY_Axis (myLinearAxis "f(x)" false)
|> Chart.withTitle "cos / sin (x)"

//1.4b: Die zusätzliche Dimension entsteht durch die Farbgebung für die unterschiedlichen Traces im Plot.
(**
### Task 1.5 

Visualisieren Sie die (x,y) tuple in der folgenden Liste zunaechst mit einer linearen X und Y Skala.
Transformieren Sie die Daten mit einer nichtlinearen Transformation, die Ihnen sinnvoll erscheint und 
visualisieren Sie das Ergebnis. 

_Tipp: sollten Sie anstatt der Daten die Achse transformieren wollen:_
`Chart.withY_Axis( Axis.LinearAxis.init(StyleParam.AxisType. ...)  )`

*)
let someData = [0. .. 1. .. 10.] |> List.map (fun x -> x,(10. ** x))

//Schaut euch am besten immer zuerst die rohdaten an:
someData
|> Chart.Point

//Man sieht sehr gut wie weit die datenpunkte auseinander liegen.

//Chart.Spline interpoliert die linie zwischen den Punkten, dann sieht das ganze smoother aus als mit Chart.Line
//Hier wird die Achse logarithmiert, beachtet dass hier die achse den titel "y" behält
Chart.Spline(someData)
|> Chart.withY_Axis (Axis.LinearAxis.init(StyleParam.AxisType.Log))
|> Chart.withY_AxisStyle "y"
|> Chart.withX_AxisStyle "x"

//Hier werden die y-Werte logarithmiert, beachtet dass hier die achse den titel "log(y)" erh�lt.
let transformedData = 
    someData
    |> List.map (fun (x,y) -> (x,log y))

Chart.Spline(transformedData)
|> Chart.withX_AxisStyle "x"
|> Chart.withY_AxisStyle "log (y)"
(**
## 2 Visualisierung von Anzahl/Groessen

### Task 2.1 

Visualisieren Sie die Abundanz verschiedener Haarfarben der Teilnehmer des Kurses. Falls keine Erhebung 
gemacht werden soll, denken Sie sich einen Datensatz mit 5 verschiedenen Haarfarben und 20 Probanden aus.

_Tipp: erstellen Sie entweder eine Liste mit (Haarfarbe,Abundanz) tupeln oder je eine Liste fuer Haarfarbe und Abundanz_

*)
let haarfarben = [
    "Schwarz", 9
    "Rot"    , 2
    "Gelb"   , 1
    "Braun"  , 7
    "Grau"   , 1
]

Chart.Column(haarfarben)
|> Chart.withX_Axis (myLinearAxis "Farbe" false)
|> Chart.withY_Axis (myLinearAxis "Abundanz" true)
|> Chart.withTitle "random haarfarben"
(**
### Task 2.2

Visualisieren Sie die Menge an erzeugten Terawattstunden in Deutschland nach verschiedenen Energietraeger im Jahr 2020 hinsichtlich der absoluten Zahlen als Balkendiagramm, sortiert nach Groesse der Strommenge

_Quelle der Daten:_ https://de.wikipedia.org/wiki/Stromerzeugung#Bruttostromerzeugung_nach_Energietr%C3%A4gern_in_Deutschland

*)
let terawattstundenDeutschland2020 = 
    [
        91.7,"Braunkohle"           
        107.0,"Windenergie, onshore" 
        91.6 ,"Erdgas"               
        64.3,"Kernenergie"          
        42.5,"Steinkohle"           
        51.,"Photovoltaik"         
        44.4,"Biomasse"             
        24.5,"uebrige Energietraeger" 
        18.5,"Windenergie, offshore"
        18.7,"Wasserkraft"          
        5.9,"Hausmuell"            
        4.2,"Mineraloelprodukte"   
    ]
    |> List.sortBy fst


Chart.Point(terawattstundenDeutschland2020)
|> Chart.withMarginSize(Left=250.)
|> Chart.withMarkerStyle 20
|> Chart.withX_Axis (myLinearAxis "Erzeugte Energie [tWh]" false)
//Bei solchen plots, bei denen eine kategorische Beschriftung pro einheitenschritt vorhanden ist, kann man meist 
//die generelle achsenbeschriftung weglassen (e.g. hier muss man nicht "Energieträger" dazuschreiben, wenn das aus titel und beschriftung gut hervorgeht)
|> Chart.withY_Axis (myLinearAxis "" true)
|> Chart.withTitle "Erzeugte Terrawattstunden von verschiedenen Energietraegern in Deutschland "
|> Chart.withSize(1000.,750.)
(**
### Task 2.3 

Datenvisialisierung kann oft zur Erkennung von Mustern in einem Datensatz fuehren, die bei blosser 
Betrachtung der Werte nicht ersichtlich waeren, besonders wenn der Datensatz sehr gross und/oder 
mehrdimensional ist. 

Der vorbereitete Testdatensatz enthaelt 3 Dimensionen: die Indices des aeusseren Arrays, die Indices der 
inneren Arrays, und die tatsaechlichen Zahlenwerte in den inneren Arrays. Visualisieren Sie die drei 
Dimensionen mithilfe einer Heatmap.

Welche Information koennte in den 3 Dimensionen kodiert sein? Beschriften Sie die x und y Skalen nach Ihrer 
Einschaetzung.


*)
let data3D =
    [|
        [|2.;2.;2.;2.;2.;2.;2.;2.;2.|]
        [|2.;2.;0.;0.;1.;0.;0.;2.;2.|]
        [|2.;0.;0.;3.;3.;3.;0.;0.;2.|]
        [|2.;0.;3.;0.;0.;0.;3.;0.;2.|]
        [|2.;0.;3.;0.;0.;0.;3.;0.;2.|]
        [|2.;0.;0.;0.;3.;0.;0.;0.;2.|]
        [|2.;0.;0.;0.;0.;0.;0.;0.;2.|]
        [|2.;0.;3.;0.;0.;0.;3.;0.;2.|]
        [|2.;2.;0.;0.;0.;0.;0.;2.;2.|]
        [|2.;2.;2.;1.;1.;1.;2.;2.;2.|]
    |]

Chart.Heatmap(data=data3D)
|> Chart.withX_AxisStyle(title="X-Koordinate des Pixels")
|> Chart.withY_AxisStyle(title="Y-Koordinate des Pixels")
|> Chart.withColorBarStyle("Pixel Intensität")

//Ich habe die intensität von bildpixeln in zahlen umgewandelt und das als 2D-Array
//bereitgestellt. Die x/y achsen sind die horizontalen/vertikalen pixel indices und die farbskala bildet die pixelintensität ab.
(**
## 3   Visualisierung von Proportionen

### Task 3.1 

Visualisieren Sie das Ergebnis der Landtagswahl in einem beliebigen Bundesland.

_Quelle der Daten:_ https://de.wikipedia.org/wiki/Liste_der_letzten_Landtagswahlergebnisse_in_Deutschland

*)
let wahlergebnisse,parteinamen,farben =
    [
        (27.7,"CDU/CSU","black")
        (35.7,"SPD","#E3000F")
        (9.3 ,"Grüne", "#46962B")
        (8.3 ,"Afd", "#009EE0")
        (5.5 ,"FDP", "#FFFF00")
        (2.5 ,"Linke", "#BE3075")
        (5.4 ,"Freie Wähler/BVB/FW", "#FF8000")
        (1.1 ,"Die PARTEI", "darkred")
        (0.5 ,"Piraten", "orange")
        (1.7 ,"Tierschutzpartei", "grey")
        (0.7 ,"ÖDP", "grey")
        (2.4 ,"sonstige", "grey")
    ]
    |> List.unzip3


Chart.Pie(values=wahlergebnisse,Labels=parteinamen)
//Hier das setzen der Farben, wie wir es in der �bung besprochen haben. 
//Auch das ist in der neuen Version deutlich einfacher.
|> GenericChart.mapTrace( fun trace ->
    let tmp = trace
    let marker = Marker.init()
    marker?colors <- farben
    trace?marker <- marker
    tmp
) 
|> Chart.withTitle "Wahlergebnisse Landtagswahl RLP 2021"
|> Chart.withSize (750.,750.)
(**
### Task 3.2 
Vergleichen Sie die Stimmanteile aus 3.1 visuell mit mindestens 3 weiteren Landtagswahlergebnissen aus 
anderen Bundeslaendern.

*)
//Relativ straight forward der code aus der Vorlesung für grouped Columns:
let datenLandtagswahl =
    [
        "Rheinlandpfalz 2021",
        [
            "CDU/CSU"               , 27.
            "SPD"                   , 35.
            "Grüne"                 , 9.3
            "Afd"                   , 8.3
            "FDP"                   , 5.5
            "Linke"                 , 2.5
            "Freie Wähler/BVB/FW"   , 5.4
            "Die PARTEI"            , 1.1
            "Piraten"               , 0.5
            "Tierschutzpartei"      , 1.7
            "ÖDP"                   , 0.7
            "sonstige"              , 2.4
        ]
        "Saarland 2017",
        [
            "CDU/CSU"                   ,   40.7
            "SPD"                       ,   29.6
            "Grüne"                     ,   4.0
            "Afd"                       ,   6.2
            "FDP"                       ,   3.3
            "Linke"                     ,   12.8
            "Freie Wähler/BVB/FW"      ,   0.4 
            "Piraten"                   ,   0.7  
            "Die PARTEI"                ,   0.  
            "Tierschutzpartei"          ,   0.  
            "sonstige"                  ,   2.3
        ]
        "Baden-Würtemberg 2021",
        [
            "CDU/CSU"               , 24.1
            "SPD"                   , 11.
            "Grüne"                 , 32.6
            "Afd"                   , 9.7
            "FDP"                   , 10.5
            "Linke"                 , 3.6
            "Freie Wähler/BVB/FW"   , 3.
            "Die PARTEI"            , 1.2
            "Piraten"               , 0.3
            "Tierschutzpartei"      , 0.
            "ÖDP"                   , 0.8
            "sonstige"              , 4.2
        ]
        "Sachsen 2019",
        [
            "CDU/CSU"               ,   32.1
            "SPD"                   ,   7.7 
            "Grüne"                 ,   8.6 
            "Afd"                   ,   27.5
            "FDP"                   ,   4.5 
            "Linke"                 ,   10.4
            "Freie Wähler/BVB/FW"  ,   3.4 
            "Piraten"               ,   0.3 
            "Die PARTEI"            ,   1.6 
            "Tierschutzpartei"      ,   1.5
            "ÖDP"                   ,   0.3
            "sonstige"              ,   2.4

        ]
    ]

let landtagswahlenChart=
    datenLandtagswahl
    |> List.map
        (fun (gruppenName, daten)-> 
            Chart.Column(keysvalues=daten, Name=gruppenName))
    |> Chart.Combine
    |> Chart.withTitle "Landtagwahlverteilung"
    |> Chart.withY_AxisStyle ("Wahlergebnis in %", MinMax = (0.,50.))

landtagswahlenChart
|> Chart.withSize(1000.,1000.)
(**
### Task 3.3 
Visualisieren Sie die Daten aus 2.2 hinsichtlich ihres relativen Anteils an der Gesamtstromerzeugung als Doughnut-Chart 

_Tipp: Chart.Doughnut; Sie muessen die Ursprungsdaten jeweils durch die Gesamtmenge an erzeugten kWh Teilen._

*)
let relativeTerawattstundenDeutschland2020 =
    terawattstundenDeutschland2020
    |> fun l ->
        let sum = List.sumBy fst l
        l |> List.map (fun (terrawatt,erzeuger) -> terrawatt/sum , erzeuger)

Chart.Doughnut(data=relativeTerawattstundenDeutschland2020)
|> Chart.withTitle "relative Terawattstundenproduktion in Deutschland je Energieträger (2020)"
|> Chart.withSize (750.,750.)
(**
### 4 Visualisierung von geographischer Lage

### Task 4.1

Visualisieren Sie den Anteil der 16 bevoelkerungsreichsten Laender an der Gesamtbevoelkerung der Erde. 

_Quelle der Daten:_ https://de.wikipedia.org/wiki/Weltbev%C3%B6lkerung#Die_bev%C3%B6lkerungsreichsten_Staaten

_Tipp: Chart.Choropleth. Idealerweise aehnelt ihr Ergebnis der Abbildung auf Wikipedia._

*)
// Die Ländernamen müssen english sein.
// Ansonsten straight forward aus der Vorlesung:

let locations,z = 
    [
        ("China", 18.2);
        ("India", 18.1);
        ("USA", 5.1);
        ("Indonesia",3.7);
        ("Pakistan",2.8);
        ("Brazil", 2.7);
        ("Nigeria", 2.6);
        ("Bangladesh", 2.1);
        ("Russia",1.9);
        ("Mexico", 1.7);
        ("Japan", 1.7);
        ("Ethopia", 1.5);
        ("Philippines", 1.4);
        ("Egypt", 1.3);
        ("Vietnam", 1.2);
        ("Deutschland", 1.1);
    ]
    |> List.unzip

Chart.ChoroplethMap(locations,z,Locationmode=StyleParam.LocationFormat.CountryNames)
|> Chart.withTitle "Die Bevölkerungsreichsten Staaten der Welt (% Gesamtbevölkerung 2019)"
|> Chart.withSize(1000.,1000.)
(**
## 5 Bonus

Erstellen Sie ein Klimadiagramm fuer Kaiserslautern
Hierzu muessen verschiedene Charttypen miteinander kombiniert werden.

_Quelle der Daten:_ https://de.climate-data.org/europa/deutschland/rheinland-pfalz/kaiserslautern-2135/


*)
let meanTemp = [1.2; 1.8; 5.2; 9.6; 13.6; 17.2; 19.; 18.6; 14.8; 10.4; 5.5; 2.3]

let precipitation = [64; 57; 62; 60; 79; 74; 78; 70; 66; 67; 68; 78]

let months = ["Januar" ; "Februar"; "März"; "April"; "Mai"; "Juni"; "Juli"; "August"; "September"; "Oktober"; "November"; "Dezember"]

//Temperatur kurve ähnlich der seite von der die daten kommen:

let tempChart = 
    Chart.Line (
        months,
        meanTemp, 
        Name = "Durchschnittstemperatur",
        ShowMarkers=true
        )
    |> Chart.withLineStyle(Width = 10.,Shape=StyleParam.Shape.Spline)
    |> Chart.withMarkerStyle(Size = 15)


[
    Chart.Column (months, precipitation, Name = "Niederschlag")
    |> Chart.withAxisAnchor(Y=1);
    tempChart
    |> Chart.withAxisAnchor(Y=2)
]
|> Chart.Combine 
|> Chart.withY_AxisStyle("Precipitation (mm)", Side=StyleParam.Side.Left, Id=1, MinMax = (0.,80.)) 
|> Chart.withY_AxisStyle("Mean temperature (°C)",Side=StyleParam.Side.Right, Id=2, Overlaying=StyleParam.AxisAnchorId.Y 1, MinMax = (0.,20.))
|> Chart.withTitle "Klimadiagramm"
|> Chart.withX_AxisStyle "Monate"
|> Chart.withSize(1000.,1000.)