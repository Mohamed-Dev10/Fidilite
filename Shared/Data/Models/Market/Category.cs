using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class Category
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public int Position { get; set; }

    public int Level { get; set; }

    public int ProductCount { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category Parent { get; set; }
}
