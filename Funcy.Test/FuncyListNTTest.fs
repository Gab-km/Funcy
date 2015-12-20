namespace Funcy.Test

open System
open Funcy
open Persimmon
open UseTestNameByReflection

module FuncyListNTTest =

    // Take
    let ``FuncyListNT.Take(cons, 3) returns a value of type Cons`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.Take(3)
        do! assertEquals typeof<Cons<string>> <| sut.GetType()
    }
    let ``FuncyListNT.Take(nil, 3) returns a value of type Nil`` = test {
        let flist = FuncyList<string>.Construct([||])
        let sut = flist.Take(3)
        do! assertEquals typeof<Nil<string>> <| sut.GetType()
    }

    let ``FuncyListNT.Take(list, 3) takes first 3 elements`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.Take(3)
        do! assertEquals sut <| FuncyList.Construct([| "my"; "name"; "is" |])
    }
    let ``FuncyListNT.Take(list, 3) returns list when the length of list is less than or equal to 3`` = test {
        let flist = FuncyList.Construct([| "FuncyList" |])
        let sut = flist.Take(3)
        do! assertEquals sut <| flist
    }
    let ``FuncyListNT.Take(list, 0) returns Nil`` = test {
        let flist = FuncyList.Construct([| "FuncyList" |])
        let sut = flist.Take(0)
        do! assertEquals sut <| (FuncyList<string>.Nil() :> FuncyList<string>)
    }
    
    let ``FuncyListNT.Take commutes with Length function`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let func = Func<string, int>(fun s -> s.Length)
        do! assertEquals <|| (flist.Take(3).FMap(func), flist.FMap(func).Take(3))
    }

    // TakeFirst
    let ``FuncyListNT.TakeFirst(cons) returns a value of type Cons`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.TakeFirst()
        do! assertEquals typeof<Some<string>> <| sut.GetType()
    }
    let ``FuncyListNT.TakeFirst(nil) returns a value of type Nil`` = test {
        let flist = FuncyList<string>.Construct([||])
        let sut = flist.TakeFirst()
        do! assertEquals typeof<None<string>> <| sut.GetType()
    }

    let ``FuncyListNT.TakeFirst(list) takes a first element`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.TakeFirst()
        do! assertEquals sut <| (Maybe.Some("my") :> Maybe<string>)
    }

    let ``FuncyListNT.TakeFirst commutes with Length function`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let func = Func<string, int>(fun s -> s.Length)
        do! assertEquals <|| (flist.TakeFirst().FMap(func), flist.FMap(func).TakeFirst())
    }
