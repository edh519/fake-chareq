using BusinessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PullRequestService
    {
        public string ExtractChaReqApprovalMessage(Review review)
        {
            string result = string.Empty;

            if (review != null)
            {
                string searchTerm = "#ChaReqMessage";

                // Searches for a string after #ChaReqMessage
                int index = review.body.IndexOf(searchTerm);
                if (index != -1)
                {
                    result = review.body.Substring(index + searchTerm.Length).Trim();
                }
            }
            return result;
        }
        public List<int> ExtractLinkedIssues(string prBody)
        {
            // RegEx to match each 'closes' followed by one or more issue numbers (#59, #61, etc.)
            string pattern = @"\bcloses\b(?:\s+#\d+|\s*,\s*#\d+|\s+and\s+#\d+)*";
            MatchCollection matches = Regex.Matches(prBody, pattern, RegexOptions.IgnoreCase);

            List<int> issueNumbers = new List<int>();

            foreach (Match match in matches)
            {
                // Extract all individual issue numbers within the matched 'closes' clause
                MatchCollection issueMatches = Regex.Matches(match.Value, @"#(\d+)");
                foreach (Match issueMatch in issueMatches)
                {
                    issueNumbers.Add(int.Parse(issueMatch.Groups[1].Value));
                }
            }

            return issueNumbers;
        }


    }
}
