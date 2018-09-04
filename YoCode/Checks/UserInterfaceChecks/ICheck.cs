using System.Collections.Generic;

namespace YoCode
{
    internal interface ICheck
    {
        IEnumerable<FeatureEvidence> Execute();
    }
}