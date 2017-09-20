using System;
using IE.CommonSrc.IEIntegration;

namespace IE.CommonSrc.ExternalDB
{
    public class ExternalDBCommand
    {
        public string requestingUsername { get; set; }
        public IEMember member { get; set; }
    }
}
