using System.Text.Json.Serialization;

namespace JuniperMarket.Models.Tax
{
    public class JurisdictionBreakdown
    {

        [JsonPropertyName("taxable_amount")]
        public float TaxableAmount { get; set; }

        [JsonPropertyName("tax_collectable")]
        public float TaxCollectable { get; set; }

        [JsonPropertyName("combined_tax_rate")]
        public float CombinedTaxRate { get; set; }

        [JsonPropertyName("state_taxable_amount")]
        public float stateTaxableAmount { get; set; }

        [JsonPropertyName("state_tax_rate")]
        public float StateTaxRate { get; set; }

        [JsonPropertyName("state_tax_collectable")]
        public float StateTaxCollectable { get; set; }


    }
}
