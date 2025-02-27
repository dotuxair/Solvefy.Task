﻿using System.ComponentModel.DataAnnotations;

namespace Solvefy.Task.Models.Dtos;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}