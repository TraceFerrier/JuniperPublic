using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace JuniperMarket.Models.Tax
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class TaxRates
    {
        [JsonPropertyName("zip")]
        public string Zip { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("country_rate")]
        public float CountryRate { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("state_rate")]
        public float StateRate { get; set; }

        [JsonPropertyName("county")]
        public string County { get; set; }

        [JsonPropertyName("county_rate")]
        public float CountyRate { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("city_rate")]
        public float CityRate { get; set; }

        [JsonPropertyName("combined_district_rate")]
        public float CombinedDistrictRate { get; set; }

        [JsonPropertyName("combined_rate")]
        public float CombinedRate { get; set; }

        [JsonPropertyName("freight_taxable")]
        public bool FreightTaxable { get; set; }

        // International
        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Australia / SST States

        // European Union
        [JsonPropertyName("standard_rate")]
        public decimal StandardRate { get; set; }

        [JsonPropertyName("reduced_rate")]
        public decimal ReducedRate { get; set; }

        [JsonPropertyName("super_reduced_rate")]
        public decimal SuperReducedRate { get; set; }

        [JsonPropertyName("parking_rate")]
        public decimal ParkingRate { get; set; }

        [JsonPropertyName("distance_sale_threshold")]
        public decimal DistanceSaleThreshold { get; set; }
    }

    public class TaxRateResponse
    {
        [JsonPropertyName("rate")]
        public TaxRates Rate { get; set; }
    }
}
