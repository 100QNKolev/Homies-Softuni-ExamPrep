using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Homies.Data
{
    public class DataConstants
    {
        //Constans for Event class

        public const int EventNameMininmumLentgth = 5;
        public const int EventNameMaximumLentgth = 20;

        public const int EventDescriptionMininmumLentgth = 15;
        public const int EventDescriptionMaximumLentgth = 150;

        public const string EventDateTimeFormat = "yyyy-MM-dd H:mm";

        public const string RequireErrorMessage = "The field {0} is required";
        public const string StringLengthErrorMessage = "The field {0} must be between {2} and {1} characters long";
        public const string DateTimeError = $"Invalid date! Format must be: {EventDateTimeFormat}";

        //------------------------------------------------------------------------
        //Constants for Type class

        public const int TypeNameMininmumLentgth = 5;
        public const int TypeNameMaximumLentgth = 15;

    }
}
