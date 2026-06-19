using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class GlovoCatalogue
{
    public string ExternalId { get; set; }

    public string ProductName { get; set; }

    public string SuperCollection { get; set; }

    public double? SuperCollectionOrder { get; set; }

    public string SuperCollectionImage { get; set; }

    public string Collection { get; set; }

    public string CollectionImage { get; set; }

    public double? CollectionOrder { get; set; }

    public string Section { get; set; }

    public double? SectionOrder { get; set; }

    public double? Price { get; set; }

    public string Image1 { get; set; }

    public string ImageSource1 { get; set; }

    public string Image2 { get; set; }

    public string ImageSource2 { get; set; }

    public string Image3 { get; set; }

    public string ImageSource3 { get; set; }

    public string Image4 { get; set; }

    public string ImageSource4 { get; set; }

    public string Image5 { get; set; }

    public string ImageSource5 { get; set; }

    public string Image6 { get; set; }

    public string ImageSource6 { get; set; }

    public string Image7 { get; set; }

    public string ImageSource7 { get; set; }

    public string Image8 { get; set; }

    public string ImageSource8 { get; set; }

    public string Image9 { get; set; }

    public string ImageSource9 { get; set; }

    public string Image10 { get; set; }

    public string ImageSource10 { get; set; }

    public string Description { get; set; }

    public string IsAlcoholic { get; set; }

    public string IsTobacco { get; set; }

    public string AttributeGroups { get; set; }

    public string Dietary { get; set; }

    public DateTime? DateExportaion { get; set; }

    public DateTime? DateCreation { get; set; }

    public DateTime? DateModification { get; set; }

    public int? Statut { get; set; }

    public int? IsModified { get; set; }

    public int? IsDeleted { get; set; }
}
