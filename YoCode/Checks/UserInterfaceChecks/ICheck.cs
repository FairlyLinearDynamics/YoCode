using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoCode
{
    internal interface ICheck
    {
        Task<List<FeatureEvidence>> Execute();
    }
}