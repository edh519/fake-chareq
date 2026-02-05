using DataAccessLayer.Models;
using Enums.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BusinessLayer.Helpers
{
    public static class CommonHelpers
    {
        /// <summary>
        /// Adds a space before every capital letter within a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SpaceStringAtCapitals(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            string[] array = Regex.Split(input, @"(?<!^)(?=[A-Z])");

            return string.Join(' ', array);
        }
        /// <summary>
        /// Removes the domain from the email by splitting on '@'.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string RemoveDomainFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "";

            return email.Split('@')[0];
        }
        /// <summary>
        /// Converts the WorkRequestStatus to a percentage complete.
        /// If the status name is "Abandoned" returns -100.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static double ConvertWorkRequestStatusToProgress(WorkRequestStatus status)
        {
            if (status == null)
                return 0d;

            double progress = status.WorkRequestStatusId switch
            {
                WorkRequestStatusEnum.Abandoned => -100d, // Negative to trigger red progress bar.
                WorkRequestStatusEnum.PendingInitialApproval => 10d,
                WorkRequestStatusEnum.PendingRequester => 10d,
                WorkRequestStatusEnum.PendingCompletion => 50d,
                WorkRequestStatusEnum.Completed => 100d,
                _ => 0d,
            };
            return progress;
        }

        /// <summary>
        /// Converts the SubTaskStatus to a percentage complete.
        /// If the status name is "Abandoned" returns -100.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static double ConvertSubTaskStatusToProgress(SubTaskStatus status)
        {
            if (status == null)
                return 0d;

            double progress = status.SubTaskStatusId switch
            {
                SubTaskStatusEnum.Abandoned => 100d,
                SubTaskStatusEnum.Open => 50d,
                SubTaskStatusEnum.Rejected => -100d, // Negative to trigger red progress bar.
                SubTaskStatusEnum.Approved => 100d,
                _ => 0d,
            };
            return progress;
        }

        /// <summary>
        /// Calculates whether white or black text will contast better on a given background colour.
        /// This is an estimate, and could be improved on.
        /// Calculated based on percieved brightness of colours to the human eye, and compared.
        /// This functionality is duplicated client-side in Helpers.js > CalculateBestTextColor(string hexColor)
        /// See: https://stackoverflow.com/questions/3942878/how-to-decide-font-color-in-white-or-black-depending-on-background-color
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static string CalculateBestTextColor(string hexColor)
        {
            NumberStyles hexNumberStyle = NumberStyles.HexNumber;

            string color = (hexColor.StartsWith('#')) ? hexColor.Substring(1, 6) : hexColor;

            int r = int.Parse(color.Substring(0, 2), style: hexNumberStyle); // hexToR
            int g = int.Parse(color.Substring(2, 2), style: hexNumberStyle); // hexToG
            int b = int.Parse(color.Substring(4, 2), style: hexNumberStyle); // hexToB


            return (((r * 0.299) + (g * 0.587) + (b * 0.114)) > 186) ?
                "#000000" : "#ffffff";
        }

        /// <summary>
        /// Shortens a <paramref name="rawString"/> to <paramref name="maxLength"/> and appends <paramref name="appendString"/> if shortened.
        /// </summary>
        /// <param name="rawString">String to be shortened if required.</param>
        /// <param name="maxLength">Maximum allowable <paramref name="rawString"/> to not be shortened.</param>
        /// <param name="appendString">String to append to indicate shortening. Defaults to "...".</param>
        /// <returns></returns>
        public static string TrimStringToMaxLength (string rawString, int maxLength = 99, string appendString = "...") {
            if (rawString.Length > maxLength)
            {
                return rawString[..maxLength] + appendString;
            }
            else
            {
                return rawString;
            }
        }

        public static string SalutationFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "";

            string name = RemoveDomainFromEmail(email);

            string firstName = name.Split('.')[0];

            TextInfo textInfo = new CultureInfo("en-GB").TextInfo;
            string salutation = textInfo.ToTitleCase(firstName.ToLower());

            return salutation;
        }
    }
}
