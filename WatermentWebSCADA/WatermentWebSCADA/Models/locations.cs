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
    
    public partial class locations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public locations()
        {
            this.facilities = new HashSet<facilities>();
            this.User = new HashSet<User>();
        }
    
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public Nullable<int> Postcode { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public int countries_Id { get; set; }
        public int countries_continents_Id { get; set; }
    
        public virtual countries countries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<facilities> facilities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User { get; set; }
    }
}
