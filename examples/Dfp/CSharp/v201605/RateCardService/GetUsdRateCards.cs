// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201605;
using Google.Api.Ads.Dfp.v201605;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201605 {
  /// <summary>
  /// This example gets all rate cards that have a currency in US dollars.
  /// </summary>
  public class GetUsdRateCards : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all rate cards that have a currency in US dollars.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetUsdRateCards codeExample = new GetUsdRateCards();
      Console.WriteLine(codeExample.Description);

      codeExample.Run(new DfpUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser user) {
      RateCardService rateCardService =
          (RateCardService) user.GetService(DfpService.v201605.RateCardService);

      // Create a statement to select rate cards.
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("currencyCode = :currencyCode")
          .OrderBy("id ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
          .AddValue("currencyCode", "USD");

      // Retrieve a small amount of rate cards at a time, paging through
      // until all rate cards have been retrieved.
      RateCardPage page = new RateCardPage();
      try {
        do {
          page = rateCardService.getRateCardsByStatement(statementBuilder.ToStatement());

          if (page.results != null) {
            // Print out some information for each rate card.
            int i = page.startIndex;
            foreach (RateCard rateCard in page.results) {
              Console.WriteLine("{0}) Rate card with ID \"{1}\", name \"{2}\", "
                  + "and currency code \"{3}\" was found.",
                  i++,
                  rateCard.id,
                  rateCard.name,
                  rateCard.currencyCode);
            }
          }

          statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
        } while (statementBuilder.GetOffset() < page.totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", page.totalResultSetSize);
      } catch (Exception e) {
        Console.WriteLine("Failed to get rate cards. Exception says \"{0}\"",
            e.Message);
      }
    }
  }
}
