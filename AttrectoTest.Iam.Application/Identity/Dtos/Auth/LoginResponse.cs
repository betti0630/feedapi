﻿using System.ComponentModel.DataAnnotations;

namespace AttrectoTest.Iam.Application.Identity.Dtos.Auth
{
    public record LoginResponse
    {
        public LoginResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

        [Required(AllowEmptyStrings = true)]
        public string AccessToken { get; set; }

    }


}
