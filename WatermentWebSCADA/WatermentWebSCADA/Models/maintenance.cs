//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WatermentWebSCADA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class maintenance
    {
        public int OrderID { get; set; }
        public string Person { get; set; }
        public string facilities_Name { get; set; }
    
        public virtual facilities facilities { get; set; }
    }
}
