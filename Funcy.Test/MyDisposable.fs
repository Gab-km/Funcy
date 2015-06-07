namespace Funcy.Test

type MyDisposable<'T when 'T: equality>(value : 'T) =
    member self.Value = value
    member val Disposed = false with get, set
    override self.Equals (other: obj) =
        if (other.GetType() <> typeof<MyDisposable<'T>>) then false
        else
            self.Value = (other :?> MyDisposable<'T>).Value
    override self.GetHashCode() = value.GetHashCode()
    interface System.IDisposable with
        member self.Dispose() = self.Disposed <- true
