using System;

using R5T.T0131;


namespace R5T.L0083.F001
{
    [ValuesMarker]
    public partial interface IBranchNames : IValuesMarker
    {
        /// <summary>
        /// <para><value>main</value></para>
        /// </summary>
        public string Main => "main";

        /// <summary>
        /// <para><value>master</value></para>
        /// Old name for main branch.
        /// </summary>
        public string Master => "master";
    }
}
