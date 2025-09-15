using System;

using F10Y.T0011;

using R5T.T0131;


namespace R5T.L0083.F001
{
    [ValuesMarker]
    public partial interface IBranchNames : IValuesMarker
    {
        /// <summary>
        /// <para><value>main</value></para>
        /// </summary>
        [InstanceIdentity("1C1467CE-2A2B-4FEB-AC50-4BE68718CD3F")]
        public string Main => "main";

        /// <summary>
        /// <para><value>master</value></para>
        /// Old name for main branch.
        /// </summary>
        [InstanceIdentity("9F698146-0221-4759-A5F1-55069A37D017")]
        public string Master => "master";
    }
}
