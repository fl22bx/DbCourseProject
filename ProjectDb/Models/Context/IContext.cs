using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDb.Models.Context
{
    public interface IContext
    {
        List<Sections> SectionSet(string SqlQuery);
    }
}
