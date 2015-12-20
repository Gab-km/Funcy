namespace Funcy.Test

open System
open Funcy
open Persimmon
open UseTestNameByReflection

module NonEmptyListNTTest =

    // ToFuncyList
    let ``Original NonEmptyList and resulted FuncyList have a same type of elements`` = test {
        let nel = NonEmptyList.Construct([| "my"; "elements"; "are"; "type"; "of"; "string"|])
        let sut = nel.ToFuncyList()
        do! assertEquals typeof<Cons<string>> <| sut.GetType()
    }
    let ``Original NonEmptyList and resulted FuncyList have a equivalent elements`` = test {
        let elements = [| "my"; "elements"; "are"; "type"; "of"; "string"|]
        let nel = NonEmptyList.Construct(elements)
        let fl = FuncyList.Construct(elements)
        let sut = nel.ToFuncyList()
        do! assertEquals sut fl
    }

    // Take
    let ``NonEmptyListNT.Take(cons, 3) returns a value of type Cons`` = test {
        let nel = NonEmptyList.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
        let sut = nel.Take(3)
        do! assertEquals typeof<Cons<string>> <| sut.GetType()
    }

    let ``NonEmptyListNT.Take(list, 3) takes first 3 elements`` = test {
        let nel = NonEmptyList.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
        let sut = nel.Take(3)
        do! assertEquals sut <| FuncyList.Construct([| "my"; "name"; "is" |])
    }
    let ``NonEmptyListNT.Take(list, 3) returns list.ToFuncyList() when the length of list is less than or equal to 3`` = test {
        let nel = NonEmptyList.Construct([| "NonEmptyList" |])
        let sut = nel.Take(3)
        do! assertEquals sut <| nel.ToFuncyList()
    }
    let ``NonEmptyListNT.Take(list, 0) returns Nil`` = test {
        let nel = NonEmptyList.Construct([| "NonEmptyList" |])
        let sut = nel.Take(0)
        do! assertEquals sut <| (FuncyList<string>.Nil() :> FuncyList<string>)
    }
    
    let ``NonEmptyListNT.Take commutes with Length function`` = test {
        let nel = NonEmptyList.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
        let func = Func<string, int>(fun s -> s.Length)
        do! assertEquals <|| (nel.Take(3).FMap(func), nel.FMap(func).Take(3))
    }

    // TakeFirst
    let ``NonEmptyListNT.TakeFirst<T>(cons) returns a value of type T`` = test {
        let nel = NonEmptyList.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
        let sut = nel.TakeFirst()
        do! assertEquals typeof<string> <| sut.GetType()
    }

    let ``NonEmptyListNT.TakeFirst(list) takes a first element`` = test {
        let nel = NonEmptyList.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
        let sut = nel.TakeFirst()
        do! assertEquals sut <| "my"
    }

    let ``NonEmptyListNT.TakeFirst commutes with Length function`` = test {
        let nel = NonEmptyList.Construct([| "my"; "name"; "is"; "NonEmptyList" |])
        let func = Func<string, int>(fun s -> s.Length)
        do! assertEquals <|| (func.Invoke(nel.TakeFirst()), nel.FMap(func).TakeFirst())
    }
