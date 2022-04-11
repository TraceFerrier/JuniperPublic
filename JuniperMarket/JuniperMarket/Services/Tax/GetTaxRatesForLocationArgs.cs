using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace JuniperMarket.Services.Tax
{
    public class GetTaxRatesForLocationArgs : GetTaxRatesForLocationOptionalArgs
    {
        /// <summary>
        /// Required for US rates - ignored if an international country code is specified.
        /// </summary>
        [AliasAs("zip")]
        public string Zip { get; set; }
    }

    public class GetTaxRatesForLocationOptionalArgs
    {
        /// <summary>
        /// Required for international rate queries.
        /// </summary>
        [AliasAs("country")]
        public string Country { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        [AliasAs("state")]
        public string State { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        [AliasAs("city")]
        public string City { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        [AliasAs("street")]
        public string Street { get; set; }

        public GetTaxRatesForLocationOptionalArgs() { }

        public GetTaxRatesForLocationOptionalArgs(GetTaxRatesForLocationArgs args)
        {
            Country = args.Country;
            State = args.State;
            City = args.City;
            Street = args.Street;
        }
    }
}
