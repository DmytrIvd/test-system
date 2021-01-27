using System.Collections.Generic;
using System.Linq;
using TestLibrary;

namespace TestDesigner
{
    public static class Extensions{
    public static bool IsVariantsHaveOneRight(this IEnumerable<Variant> variants){
            return variants.Any(x => x.IsRight == true);
    }
    }
}
