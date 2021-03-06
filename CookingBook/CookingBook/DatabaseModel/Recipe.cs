//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CookingBook.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Recipe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recipe()
        {
            this.Ingredients = new HashSet<Ingredient>();
        }
    
        public byte[] Picture { get; set; }
        public long ID { get; set; }
        public Nullable<long> AuthorID { get; set; }
        public Nullable<long> RecipeCategoryID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    
        public virtual Author Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual RecipeCategory RecipeCategory { get; set; }

        public override string ToString()
        {
            using (var contex = new CookingBookEntities1())
            {
                return Title + " / " + contex.Authors.Where(x => x.ID == AuthorID).FirstOrDefault().Name;
            }
        }
    }
}
