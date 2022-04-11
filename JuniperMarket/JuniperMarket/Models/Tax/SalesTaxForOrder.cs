using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace JuniperMarket.Models.Tax
{

    public class SalesTaxForOrder
    {
        [JsonPropertyName("order_total_amount")]
        public float OrderTotalAmount { get; set; }

        [JsonPropertyName("shipping")]
        public float Shipping { get; set; }

        [JsonPropertyName("taxable_amount")]
        public float TaxableAmount { get; set; }

        [JsonPropertyName("amount_to_collect")]
        public float AmountToCollect { get; set; }

        [JsonPropertyName("rate")]
        public float Rate { get; set; }

        [JsonPropertyName("has_nexus")]
        public bool HasNexus { get; set; }

        [JsonPropertyName("freight_taxable")]
        public bool FreightTaxable { get; set; }

        [JsonPropertyName("tax_source")]
        public string TaxSource { get; set; }

        [JsonPropertyName("exemption_type")]
        public string ExemptionType { get; set; }

        [JsonPropertyName("jurisdictions")]
        public Jurisdictions Jurisdictions { get; set; }

        [JsonPropertyName("breakdown")]
        public JurisdictionBreakdown Breakdown { get; set; }
    }

    public class SalesTaxForOrderResponse
    {
        [JsonPropertyName("tax")]
        public SalesTaxForOrder TaxForOrder { get; set; }
    }
}
