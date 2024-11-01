﻿using System.ComponentModel.DataAnnotations;
using SourceControlAPI.Constants;

namespace SourceControlApiV2.DTOs.Repository
{
    public class RepositoryDTO
    {
        [Required(ErrorMessage = CommonErrorMessages.RequiredName)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = CommonErrorMessages.NameLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = CommonErrorMessages.RequiredDescription)]
        [StringLength(300, MinimumLength = 20, ErrorMessage = CommonErrorMessages.LengthDescription)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RepositoryErrorMessages.VisibilityRequired)]
        public bool IsPublic { get; set; }
    }
}
