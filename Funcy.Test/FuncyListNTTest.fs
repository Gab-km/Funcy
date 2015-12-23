namespace Funcy.Test

open System
open Funcy
open Persimmon
open UseTestNameByReflection

module FuncyListNTTest =

    // ElementAt
    let ``FuncyListNT.ElementAt(cons, 0) returns a value of type Some<int>`` = test {
        let cons = FuncyList.Construct([|1|]);
        let sut = cons.ElementAt(0);
        do! assertEquals typeof<Some<int>> <| sut.GetType()
    }
    let ``FuncyListNT.ElementAt(cons, 0) returns Some(cons.head)`` = test {
        let cons = FuncyList.Construct([|1|]);
        let sut = cons.ElementAt(0);
        do! assertEquals sut <| (Maybe.Some(1) :> Maybe<int>)
    }
    let ``FuncyListNT.ElementAt([1, 2, 3], 1) returns a value of type Some<int>`` = test {
        let cons = FuncyList.Construct([|1; 2; 3|]);
        let sut = cons.ElementAt(1);
        do! assertEquals typeof<Some<int>> <| sut.GetType()
    }
    let ``FuncyListNT.ElementAt([1, 2, 3], 1) returns Some(2)`` = test {
        let cons = FuncyList.Construct([|1; 2; 3|]);
        let sut = cons.ElementAt(1);
        do! assertEquals sut <| (Maybe.Some(2) :> Maybe<int>)
    }
    let ``FuncyListNT.ElementAt(nil, 0) returns a value of a type None`` = test {
        let nil = FuncyList<int>.Construct([||]);
        let sut = nil.ElementAt(0);
        do! assertEquals typeof<None<int>> <| sut.GetType()
    }
    let ``FuncyListNT.ElementAt(nil, 0) returns None`` = test {
        let nil = FuncyList<int>.Construct([||]);
        let sut = nil.ElementAt(0);
        do! assertEquals sut <| (Maybe.None() :> Maybe<int>);
    }

    let ``FuncyListNT.ElementAt(cons, -1) returns a value of type None`` = test {
        let cons = FuncyList.Construct([|1|]);
        let sut = cons.ElementAt(-1);
        do! assertEquals typeof<None<int>> <| sut.GetType()
    }
    let ``FuncyListNT.ElementAt(cons, -1) returns None`` = test {
        let cons = FuncyList.Construct([|1|]);
        let sut = cons.ElementAt(-1);
        do! assertEquals sut <| (Maybe.None() :> Maybe<int>);
    }
    let ``FuncyListNT.ElementAt(nil, -1) returns a value of type None`` = test {
        let nil = FuncyList<int>.Construct([||]);
        let sut = nil.ElementAt(-1);
        do! assertEquals typeof<None<int>> <| sut.GetType()
    }
    let ``FuncyListNT.ElementAt(nil, -1) returns None`` = test {
        let nil = FuncyList<int>.Construct([||]);
        let sut = nil.ElementAt(-1);
        do! assertEquals sut <| (Maybe.None() :> Maybe<int>);
    }

    let ``FuncyListNT.ElementAt(cons, 3) returns a value of type None when the length of cons is less than 3`` = test {
        let cons = FuncyList.Construct([|1|]);
        let sut = cons.ElementAt(3);
        do! assertEquals sut <| (Maybe.None() :> Maybe<int>);
    }
    let ``FuncyListNT.ElementAt(cons, 3) returns None when the length of cons is less than 3`` = test {
        let cons = FuncyList.Construct([|1|]);
        let sut = cons.ElementAt(3);
        do! assertEquals typeof<None<int>> <| sut.GetType()
    }
    let ``FuncyListNT.ElementAt(nil, 11) returns a value of type None`` = test {
        let nil = FuncyList<int>.Construct([||]);
        let sut = nil.ElementAt(11);
        do! assertEquals typeof<None<int>> <| sut.GetType()
    }
    let ``FuncyListNT.ElementAt(nil, 11) returns None`` = test {
        let nil = FuncyList<int>.Construct([||]);
        let sut = nil.ElementAt(1);
        do! assertEquals sut <| (Maybe.None() :> Maybe<int>);
    }

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

    // First
    let ``FuncyListNT.First(cons) returns a value of type Cons`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.First()
        do! assertEquals typeof<Some<string>> <| sut.GetType()
    }
    let ``FuncyListNT.First(nil) returns a value of type Nil`` = test {
        let flist = FuncyList<string>.Construct([||])
        let sut = flist.First()
        do! assertEquals typeof<None<string>> <| sut.GetType()
    }

    let ``FuncyListNT.First(list) takes a first element`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let sut = flist.First()
        do! assertEquals sut <| (Maybe.Some("my") :> Maybe<string>)
    }

    let ``FuncyListNT.First commutes with Length function`` = test {
        let flist = FuncyList.Construct([| "my"; "name"; "is"; "FuncyList" |])
        let func = Func<string, int>(fun s -> s.Length)
        do! assertEquals <|| (flist.First().FMap(func), flist.FMap(func).First())
    }
