﻿namespace FastighetsKompassen.API.Validation
{
    public sealed class ValidationException : Exception
    {
        public List<ValidationError> Errors { get; }

        public ValidationException(List<ValidationError> errors)
        {
            Errors = errors;
        }
    }
}

   
