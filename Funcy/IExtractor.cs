using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy
{
    public interface IExtractor<T>
    {
        T Extract();
    }
}
