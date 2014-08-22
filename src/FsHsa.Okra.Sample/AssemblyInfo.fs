namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FsHsa.Okra.Sample")>]
[<assembly: AssemblyProductAttribute("FsHsa")>]
[<assembly: AssemblyDescriptionAttribute("An F# binding for HSA (Heterogeneous System Architecture)")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
